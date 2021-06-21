using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yitter.IdGenerator;

namespace Utils
{
    public static class IDHelper
    {
        static IDHelper()
        {
            var options = new IdGeneratorOptions(1);
            YitIdHelper.SetIdGenerator(options);
        }
        public static long GetId() => YitIdHelper.NextId();
    }
}
