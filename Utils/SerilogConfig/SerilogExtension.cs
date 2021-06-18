using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.SerilogConfig
{
    public static class SerilogExtension
    {
        static SerilogExtension()
        {
            Log.Logger = RegistLogger();
        }
        public static ILogger RegistLogger()
        {
            return new LoggerConfiguration()
                //配置日志最小输出的级别为：debug
                .MinimumLevel.Debug()
                //如果是Microsoft的日志，最小记录等级为info
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                //输出到控制台
                .WriteTo.Console()
                //将日志保存到文件中（两个参数分别是日志的路径和生成日志文件的频次，当前是一天一个文件）
                //.WriteTo.File(Path.Combine("logs", @"log.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public static IHostBuilder UseSerilogs(this IHostBuilder builder)
        {
            return builder.UseSerilog();
        }

        public static void UseSerilogRequestLoggings(this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging();
        }
    }
}
