using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Utils
{
    public static class ExpressionHelper
    {
        //private static IMemoryCache Cache { get; set; }
        private static Dictionary<string,object> Cache { get; set; }
        static ExpressionHelper()
        {
            //Cache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
            Cache = new Dictionary<string, object>();
        }
        /// <summary>
        /// 使用表达式树对对象进行赋值，比反射性能强
        /// </summary>
        /// <typeparam name="T">要赋值的实体类</typeparam>
        /// <typeparam name="object">要赋值的实体类</typeparam>
        /// <param name="property">要赋值的属性</param>
        /// <returns></returns>
        public static Action<T, object> GetSetter<T>(PropertyInfo property)
        {
            Action<T, object> result = null;
            Type type = typeof(T);
            string key = type.AssemblyQualifiedName + "_set_" + property.Name;
            if (Cache.TryGetValue(key,out object CacheValue))
            {
                result = CacheValue as Action<T, object>;
            }
            else
            {
                ParameterExpression parameter = Expression.Parameter(type, "t");
                ParameterExpression value = Expression.Parameter(typeof(object), "propertyValue");
                MethodInfo setter = type.GetMethod("set_" + property.Name);
                //判断是否可空
                Type t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                MethodCallExpression call = Expression.Call(parameter, setter, Expression.Convert(value, t));
                Expression<Action<T,object>> lambda = Expression.Lambda<Action<T, object>>(call, parameter, value);
                //此方法非常吃性能，必须缓存起来提高速度
                result = lambda.Compile();
                Cache.Add(key, result);
            }
            return result;
        }

        /// <summary>
        /// 使用表达式树赋值，速度不如反射快，建议不用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Func<T,object> GetGetter<T>(PropertyInfo property)
        {
            Func<T, object> result = null;
            Type type = typeof(T);
            string key = type.AssemblyQualifiedName + "_get_" + property.Name;
            if (Cache.TryGetValue(key, out object CacheValue))
            {
                result = CacheValue as Func<T, object>;
            }
            else
            {
                ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
                MemberExpression member = Expression.PropertyOrField(parameter, property.Name);
                UnaryExpression convertExpression = Expression.Convert(member, typeof(object));
                Expression<Func<T, object>> lambda = Expression.Lambda<Func<T, object>>(convertExpression, parameter);
                result = lambda.Compile();
                Cache.Add(key, result);
            }
            return result;
        }

        /// <summary>
        /// 高性能版构造对象，速度性能显著提升
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<T> ConvertDataTableToObject<T>(DataTable source) where T : class, new()
        {
            List<T> list = new List<T>();
            Type t = typeof(T);
            foreach (DataRow dr in source.Rows)
            {
                T item = new T();
                foreach (var prop in t.GetProperties())
                {
                    string MapperName = GetMapperName(prop);
                    if (string.IsNullOrEmpty(MapperName))
                    {
                        MapperName = prop.Name;
                    }
                    if (dr.Table.Columns.Contains(MapperName) && !IsDBNull(dr[MapperName]))
                    {
                        GetSetter<T>(prop)(item, dr[MapperName]);
                    }
                }
                list.Add(item);
            }
            return list;
        }


        /// <summary>
        /// 根据selector条件和value生成p=>p.propertyName == propertyValue
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <typeparam name="TResult">委托返回类型</typeparam>
        /// <param name="Selector">p=>p.propertyName</param>
        /// <param name="value">等式右值</param>
        /// <returns></returns>
        public static BinaryExpression CreateWhereConditionBySelector<T>(Expression<Func<T,object>> Selector,object value,ParameterExpression parameter)
        {
            string key = typeof(T).AssemblyQualifiedName;
            string FieldName = GetPropertyName(Selector); 
            MemberExpression member = Expression.PropertyOrField(parameter, FieldName);
            ConstantExpression constant = Expression.Constant(value);//创建常数
            return Expression.Equal(member, constant);
        }

        /// <summary>
        /// 根据表达式获取属性的名称
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="exp">表达式</param>
        /// <returns></returns>
        public static string GetPropertyName<T>(Expression<Func<T, object>> exp)
        {
            var Name = "";
            var body = exp.Body;
            if (body is UnaryExpression)
            {
                Name = ((MemberExpression)((UnaryExpression)body).Operand).Member.Name;
            }
            else if (body is MemberExpression)
            {
                Name = ((MemberExpression)body).Member.Name;
            }
            else if (body is ParameterExpression)
            {
                Name = ((ParameterExpression)body).Type.Name;
            }
            return Name;
        }

        /// <summary>
        /// 创建lambda表达式：p=>true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return p => true;
        }

        /// <summary>
        /// 创建lambda表达式：p=>false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return p => false;
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static Expression<Func<T, TKey>> GetOrderExpression<T, TKey>(string propertyName)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            return Expression.Lambda<Func<T, TKey>>(Expression.Property(parameter, propertyName), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName == propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">等式右值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateEqual<T>(string propertyName, object propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.Equal(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName != propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">等式右值</param>
        /// <returns></returns>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateNotEqual<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.NotEqual(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName > propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">等式右值</param>
        /// <returns></returns>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateGreaterThan<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName < propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">等式右值</param>
        /// <returns></returns>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateLessThan<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.LessThan(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName >= propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">等式右值</param>
        /// <returns></returns>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateGreaterThanOrEqual<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName <= propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">等式右值</param>
        /// <returns></returns>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateLessThanOrEqual<T>(string propertyName, string propertyValue, Type type)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName.Contains(propertyValue)
        /// </summary>
        //// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">等式右值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetContains<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(string));
            return Expression.Lambda<Func<T, bool>>(Expression.Call(member, method, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName.StartsWith(propertyValue)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">等式右值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetStartsWith<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(string));
            return Expression.Lambda<Func<T, bool>>(Expression.Call(member, method, constant), parameter);
        }
        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName.EndsWith(propertyValue)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">等式右值</param>
        /// <returns></returns>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetEndsWith<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(string));
            return Expression.Lambda<Func<T, bool>>(Expression.Call(member, method, constant), parameter);
        }
        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName.CompareTo(propertyValue)
        /// sqlsugar不支持CompareTo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> StringGreaterThanOrEqual<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("CompareTo", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(string));
            BinaryExpression siteNoExpression = Expression.GreaterThanOrEqual(Expression.Call(member, method, constant), Expression.Constant(0, typeof(int)));
            return Expression.Lambda<Func<T, bool>>(siteNoExpression, parameter);
        }


        /// <summary>
        /// 创建lambda表达式：!(p=>p.propertyName.Contains(propertyValue))
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetNotContains<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(string));
            return Expression.Lambda<Func<T, bool>>(Expression.Not(Expression.Call(member, method, constant)), parameter);
        }
        /// <summary>
        /// 从字典list映射值到T中
        /// </summary>
        /// <typeparam name="T">需要进行转换值的实体的类型</typeparam>
        /// <typeparam name="T1">字典实体的类型</typeparam>
        /// <param name="Entity">被转换值的实体</param>
        /// <param name="ExpWhereCondition">根据何条件对字典类进行筛选</param>
        /// <param name="MapperList">字典类list</param>
        /// <param name="PropMapper">被转换字段和字典实体字段的映射名称(没有默认相同)</param>
        /// <param name="selectors">被转换实体的字段</param>
        public static void MapperColumn<T, T1>(T Entity, Func<T1, bool> ExpWhereCondition, List<T1> MapperList, Dictionary<string, string> PropMapper, params Expression<Func<T, object>>[] selectors)
        {
            T1 SelectItem = MapperList.Where(ExpWhereCondition).FirstOrDefault();
            //如果字典类里面没有数据,直接结束本方法
            if (SelectItem == null) return;
            ParameterExpression parameter = Expression.Parameter(typeof(T1), "t");//构造参数T
            foreach (Expression<Func<T, object>> selector in selectors)
            {
                //获取属性
                string PropName = GetPropertyName(selector);
                //映射属性值
                string MapperName = string.Empty;
                if (!PropMapper.TryGetValue(PropName, out MapperName))
                {
                    MapperName = PropName;
                }
                //实体对应属性
                var EntityProp = typeof(T).GetProperty(PropName);
                //需要把NewSelector.Compile()的东西缓存下来,提升效率
                if (!Cache.TryGetValue($"{MapperName}setter", out object value))
                {
                    //根据T1构造的新selector
                    var NewSelector = Expression.Lambda<Func<T1, object>>(Expression.Property(parameter, MapperName), parameter);
                    Func<T1, object> func = NewSelector.Compile();
                    //赋值
                    GetSetter<T>(EntityProp)(Entity, func.Invoke(SelectItem));
                    //加入缓存
                    Cache.Add($"{MapperName}setter", func);
                }
                else
                {
                    var func = value as Func<T1, object>;
                    //给entity赋值
                    GetSetter<T>(EntityProp)(Entity, func.Invoke(SelectItem));
                }
            }
        }
        /// <summary>
        /// 逻辑与方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="OrginExp">源表达式</param>
        /// <param name="AndExp">拼接的表达式</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> OrginExp, Expression<Func<T, bool>> AndExp)
        {
            InvocationExpression invoke = Expression.Invoke(AndExp, OrginExp.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.And(OrginExp.Body, invoke), OrginExp.Parameters);
        }
        /// <summary>
        /// 最短路径与方法(当表达式中某段表达式可以决定表达式的最终结果就放弃接下来的所有操作)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="OrginExp"></param>
        /// <param name="OrExp"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> OrginExp, Expression<Func<T, bool>> OrExp)
        {
            InvocationExpression invoke = Expression.Invoke(OrExp, OrginExp.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.Or(OrginExp.Body, invoke), OrginExp.Parameters);
        }
        /// <summary>
        /// 逻辑或方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="OrginExp"></param>
        /// <param name="AndExp"></param>
        /// <returns></returns>

        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> OrginExp, Expression<Func<T, bool>> AndExp)
        {
            InvocationExpression invoke = Expression.Invoke(AndExp, OrginExp.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(OrginExp.Body, invoke), OrginExp.Parameters);
        }

        /// <summary>
        /// 最短路径或方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="OrginExp"></param>
        /// <param name="OrExp"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> OrginExp, Expression<Func<T, bool>> OrExp)
        {
            InvocationExpression invoke = Expression.Invoke(OrExp, OrginExp.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(OrginExp.Body, invoke), OrginExp.Parameters);
        }

        
        /// <summary>
        /// 获取标注的属性名
        /// </summary>
        /// <param name="prop">属性</param>
        /// <returns>属性名</returns>
        private static string GetMapperName(PropertyInfo prop)
        {
            var Attribute = prop.GetCustomAttribute<MapperAttribute>();
            if (Attribute == null)
            {
                return "";
            }
            if (Attribute.IgnoreColumn)
            {
                return "";
            }
            else
            {
                return Attribute.MapperName;
            }
        }

       
        private static bool IsDBNull(object t)
        {
            return t is DBNull;
        }
    }

    public class ExpressionGenericMapper<TIn, TOut>//Mapper`2
    {
        private static Func<TIn, TOut> _FUNC = null;
        static ExpressionGenericMapper()
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
            List<MemberBinding> memberBindingList = new List<MemberBinding>();
            foreach (var item in typeof(TOut).GetProperties())
            {
                MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
                MemberBinding memberBinding = Expression.Bind(item, property);
                memberBindingList.Add(memberBinding);
            }
            foreach (var item in typeof(TOut).GetFields())
            {
                MemberExpression property = Expression.Field(parameterExpression, typeof(TIn).GetField(item.Name));
                MemberBinding memberBinding = Expression.Bind(item, property);
                memberBindingList.Add(memberBinding);
            }
            MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
            Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[]
            {
                     parameterExpression
            });
            _FUNC = lambda.Compile();//拼装是一次性的
        }
        public static TOut Trans(TIn t)
        {
            return _FUNC(t);
        }
    }
}
