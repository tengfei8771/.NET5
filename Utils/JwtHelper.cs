
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class JwtHelper
    {
        private static IConfiguration Configuration { get; set; }
        private static string Key=null;
        static JwtHelper()
        {
            GetConfiguration();
        }

        public static long ToUnixEpochDate(DateTime date) =>
            (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        /// <summary>
        /// 生成jwt令牌
        /// </summary>
        /// <param name="payLoad">载荷</param>
        /// <param name="expiresMinute">过期时间</param>
        /// <param name="header">JWT的header一般不填</param>
        /// <returns></returns>
        public static string CreateToken(Dictionary<string, object> payLoad = null, int expiresMinute = 2, Dictionary<string, object> header = null)
        {
            if (Key == null)
            {
                GetConfiguration();
            }
            if (header == null)
            {
                header = new Dictionary<string, object>()
                {
                    {"alg", "HS256" },
                    {"typ", "JWT" }
                };
            }
            if (payLoad == null)
            {
                payLoad = new Dictionary<string, object>();
            }
            var now = DateTime.UtcNow;
            payLoad["nbf"] = ToUnixEpochDate(now);//可用时间起始
            payLoad["exp"] = ToUnixEpochDate(now.Add(TimeSpan.FromMinutes(expiresMinute)));//可用时间结束
            var encodedHeader = Base64UrlEncoder.Encode(JsonConvert.SerializeObject(header));
            var encodedPayload = Base64UrlEncoder.Encode(JsonConvert.SerializeObject(payLoad));
            var hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(Key));
            var encodedSignature = Base64UrlEncoder.Encode(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(encodedHeader, ".", encodedPayload))));
            var encodedJwt = string.Concat(encodedHeader, ".", encodedPayload, ".", encodedSignature);
            return encodedJwt;
        }
        /// <summary>
        /// 验证jwt令牌
        /// </summary>
        /// <param name="str">验证的字符串</param>
        /// <param name="Msg">验证信息</param>
        /// <returns></returns>
        public static bool ValidateJwt(string str,out string Msg)
        {
            try
            {
                bool flag = true;  
                string[] StrArr = str.Split('.');
                string First = Base64UrlEncoder.Decode(StrArr[0]);
                JObject JFirst = JObject.Parse(First);
                string Type = JFirst.Value<string>("alg");
                string Second = Base64UrlEncoder.Decode(StrArr[1]);
                JObject JSecond= JObject.Parse(Second);
                if (Key == null)
                {
                    //Key = Base64UrlEncoder.Encode(Configuration.GetValue<string>("SecurityKey").Trim());
                    //Key = Convert.ToBase64String(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("SecurityKey").Trim()));
                    //Key = Configuration.GetValue<string>("SecurityKey").Trim();
                    Key = Configuration.GetSection("SecurityKey").Value.Trim();
                }
                if (Type.ToUpper() == "HS256")
                {
                    var hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(Key));
                    var encodedSignature = Base64UrlEncoder.Encode(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(StrArr[0], ".", StrArr[1]))));
                    //var encodedSignature1 = Convert.ToBase64String(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(First, ",", Second))));
                    flag = flag && (StrArr[2] == encodedSignature);
                    if (flag)//自定义验证全部写在这里
                    {
                        var now = ToUnixEpochDate(DateTime.UtcNow);
                        long exp = JSecond.Value<long>("exp");
                        if (flag && (now > exp))
                        {
                            Msg = "签名已经过期!";
                            return false;
                        }            
                    }
                    else
                    {
                        Msg = "签名验证失败！";
                        return false;
                    }
                    Msg = "验证成功！";
                    return true;

                }
                else
                {
                    Msg = "未定义的加密类型!";
                    return false;
                }
            }
            catch(Exception e)
            {
                Msg = e.Message;
                return false;
            }
            
        }

        private static void GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json",optional: true, reloadOnChange: true);
            Configuration = builder.Build();
            Key = Configuration.GetSection("SecurityKey").Value.Trim();
        }
    }
}
