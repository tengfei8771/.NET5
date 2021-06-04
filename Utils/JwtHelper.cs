
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
        /// <param name="str">token令牌</param>
        /// <param name="Msg"></param>
        /// <returns>
        /// <para>
        /// 是否验证成功
        /// </para>
        /// <para>
        /// 验证的结果code
        /// </para>
        /// </returns>
        public static Tuple<bool,int> ValidateJwt(string str)
        {
            try
            {
                bool flag = true;
                var JwtData = GetJwtInfo(str);
                JObject JFirst = JObject.Parse(Base64UrlEncoder.Decode(JwtData.Item1));
                string Type = JFirst.Value<string>("alg");
                string Second = Base64UrlEncoder.Decode(JwtData.Item2);
                //用户信息段
                JObject JSecond= JObject.Parse(Second);
                if (Key == null)
                {
                    Key = Configuration.GetSection("SecurityKey").Value.Trim();
                }
                if (Type.ToUpper() == "HS256")
                {
                    var hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(Key));
                    var encodedSignature = Base64UrlEncoder.Encode(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(JwtData.Item1, ".", JwtData.Item2))));
                    //var encodedSignature1 = Convert.ToBase64String(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(First, ",", Second))));
                    flag = flag && (JwtData.Item3 == encodedSignature);
                    if (flag)//自定义验证全部写在这里
                    {
                        var now = ToUnixEpochDate(DateTime.UtcNow);
                        long exp = JSecond.Value<long>("exp");
                        if (flag && (now > exp))
                        {
                            if(now-exp<= 10 * 60 * 1000)
                            {
                                return Tuple.Create(flag, (int)ValidateType.WillOverdue);
                            }
                            
                        }
                        return Tuple.Create(flag, (int)ValidateType.Success);
                    }
                    else
                    {
                        return Tuple.Create(flag, (int)ValidateType.IllegalToken);
                    }
                }
                else
                {
                    return Tuple.Create(false, (int)ValidateType.UnkonwType);
                }
                
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 把JWT数据拆成三段返回
        /// </summary>
        /// <param name="Jwt">JWT令牌</param>
        /// <returns></returns>
        private static Tuple<string,string,string> GetJwtInfo(string Jwt)
        {
            try
            {
                string[] StrArr = Jwt.Split('.');
                return Tuple.Create(StrArr[0], StrArr[1], StrArr[2]);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
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
        public enum ValidateType
        {
            //非法令牌
            IllegalToken,
            //成功
            Success,
            //成功但即将过期
            WillOverdue,
            //已经过期
            Overdue,
            //未定义的验证类型
            UnkonwType
        }
    }
}
