using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Utils.JsonConvertHelper;

namespace IServices.ResModel
{
    public class ResponseModel
    {
        [JsonConverter(typeof(ConvertEnumToValue))]
        public ResponseTypeEnum code { get; set; }
        [JsonConverter(typeof(ConvertEnumToDescription))]
        public object message { get; set; }
        public object items { get; set; }
        public int? total { get; set; }
        public string path  { get; set; }
    }

}
