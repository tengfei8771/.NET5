using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices.ResModel
{
    public class ResponseModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Items { get; set; }
        public int? Total { get; set; }
        public string Path  { get; set; }
    }
}
