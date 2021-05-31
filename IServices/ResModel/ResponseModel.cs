using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices.ResModel
{
    public class ResponseModel
    {
        public int code { get; set; }
        public string message { get; set; }
        public object items { get; set; }
        public int? total { get; set; }
        public string path  { get; set; }
    }
}
