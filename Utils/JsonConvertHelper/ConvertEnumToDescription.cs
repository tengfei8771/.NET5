using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.JsonConvertHelper
{
    public class ConvertEnumToDescription: JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            JToken jt = JToken.ReadFrom(reader);
            Type t = Nullable.GetUnderlyingType(objectType) ?? objectType;
            object SafeValue = jt != null ? Convert.ChangeType(jt, t) : null;
            return SafeValue;
            //return ConvertObjectToType(jt, objectType);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value.GetType().IsEnum)
            {
                serializer.Serialize(writer, ReflectionConvertHelper.GetEnumDescription(value));
            }
            else
            {
                serializer.Serialize(writer, value);
            }
        }
    }
}
