using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.JsonConvertHelper
{
    public class ConvertSturtToString: JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            
            JToken jt = JToken.ReadFrom(reader);
            return ConvertObjectToType(jt, objectType);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsValueType;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Type valueType = value.GetType();
            if (valueType.GetInterface("IEnumerable") != null)
            {
                //获取集合里面的类型
                var type = valueType.GetGenericArguments()[0];
                if (type.IsValueType)
                {
                    List<string> list = new List<string>();
                    JArray arr = JArray.FromObject(value);
                    foreach(var item in arr)
                    {
                        list.Add(item.ToString());
                    }
                    serializer.Serialize(writer, list);
                }
            }
            else
            {
                serializer.Serialize(writer, value.ToString());
            }
            
        }

        private object ConvertObjectToType(JToken jt, Type objectType)
        {
            //判读是不是数组类型
            if (objectType.GetInterface("IEnumerable") != null)
            {
                //获取list里面的元素的类型
                var type= objectType.GetGenericArguments()[0];
                //如果是值类型，建立一个List 并将值add进去
                if (type.IsValueType)
                {
                    var genericTypeList = typeof(List<>).MakeGenericType(type);
                    var list= Activator.CreateInstance(genericTypeList);
                    var method = list.GetType().GetMethod("Add");
                    //List<decimal> list = new List<decimal>();
                    var arr = JArray.FromObject(jt);
                    foreach (var item in arr)
                    {
                        object value;
                        value = Convert.ChangeType(item, type);
                        object[] parameters = new object[1];
                        parameters[0] = value;
                        method.Invoke(list, parameters);
                    }
                    return list;
                }
                
                
            }
            Type t = Nullable.GetUnderlyingType(objectType) ?? objectType;
            object SafeValue = jt != null ? Convert.ChangeType(jt, t) : null;
            return SafeValue;

        }
        
    }
}
