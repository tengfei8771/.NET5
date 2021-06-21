using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace SqlSugarAndEntity
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string ClassTemplate = @"{using}
using Utils;
using Utils.JsonConvertHelper;
using Yitter.IdGenerator;
using SqlSugar;
namespace {Namespace}
{
{ClassDescription}{SugarTable}
    public partial class {ClassName}
    {
           public {ClassName}()
           {
                this.ID = IDHelper.GetId();
                {Constructor}
           }
{PropertyName}
    }
}";
                string ChildrenTemp= @"{using}
using Utils;
using System.Collections.Generic;
using SqlSugar;
namespace {Namespace}
{
{ClassDescription}{SugarTable}
    public partial class {ClassName}
    {
        public {ClassName}()
        {
           this.ID = IDHelper.GetId();
          {Constructor}
        }
{PropertyName}
           [SugarColumn(IsIgnore =true)]
           public bool hasChildren { get; set; }
           [SugarColumn(IsIgnore =true)]
           public List<{ClassName}> children { get; set; }
    }
}";
                List<string> ignoreTable = new List<string>()
                {
                    {"menuinfo" },
                    {"dictionary" },
                    {"orginfo" }
                };
                //排除不生成的表的表达式
                Expression<Func<string, bool>> exp1 = t => t.Contains('.')&&!ignoreTable.Contains(t);
                //不生成的表
                Expression<Func<string, bool>> exp2 = t => t.Contains('.')&&ignoreTable.Contains(t);
                Console.WriteLine("开始生成类库文件...");
                SqlSugarClient db = new SqlSugarClient(DataBaseConfig._config);
                var TotalPath = Directory.GetCurrentDirectory();
                string[] PathArr = TotalPath.Replace("\\", "/").Split(new string[] { "/bin/" }, StringSplitOptions.None);
                db.DbFirst.Where(exp1.Compile())
                    .SettingClassTemplate(old =>
                    {
                        return ClassTemplate;
                    })
                    .SettingNamespaceTemplate(old =>
                    {
                        return old;
                    })//命名空间 
                    .SettingPropertyTemplate(old =>
                    {
                        return old;
                    })//属性
                    .SettingConstructorTemplate(old =>
                    {
                        return old;
                    })//构造函数
                    .CreateClassFile($"{PathArr[0]}/Entity", "SqlSugarAndEntity");

                db.DbFirst.Where(t => !t.Contains('.') && (t == "menuinfo" || t == "dictionary" || t == "orginfo"))
                    .SettingClassTemplate(old =>
                    {
                        return ChildrenTemp;
                    }).CreateClassFile($"{PathArr[0]}/Entity", "SqlSugarAndEntity");
                Console.WriteLine("生成成功!");
            }
            catch(Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
            
            Console.ReadKey();
        }
    }
}
