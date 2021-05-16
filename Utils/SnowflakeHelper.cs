using Snowflake.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class SnowflakeHelper
    {
        private static IdWorker worker;
        static SnowflakeHelper()
        {
            worker = new IdWorker(1, 1);
        }
        public static long GetId() => worker.NextId();
    }
}
