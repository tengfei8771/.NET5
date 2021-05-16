using SqlSugar;
using System;
using System.IO;

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
namespace {Namespace}
{
{ClassDescription}{SugarTable}
    public partial class {ClassName}
    {
           public {ClassName}()
           {
                this.ID = SnowflakeHelper.GetId();
                {Constructor}
           }
{PropertyName}
    }
}";
                Console.WriteLine("开始生成类库文件...");
                SqlSugarClient db = new SqlSugarClient(DataBaseConfig._config);
                var TotalPath = Directory.GetCurrentDirectory();
                string[] PathArr = TotalPath.Replace("\\", "/").Split(new string[] { "/bin/" }, StringSplitOptions.None);
                db.DbFirst.Where(t=>!t.Contains('.'))
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
