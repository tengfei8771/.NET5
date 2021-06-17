using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.JsonConvertHelper
{
    public class ConvertEnumToValue: JsonConverter
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
            return objectType.IsEnum|| objectType==typeof(string);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, (int)value);
        }
    }
}
