
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private static string RequestKey;
        private static string RefreshKey;
        /// <summary>
        /// 构造函数 获取秘钥key
        /// </summary>
        static JwtHelper()=>GetConfiguration();
        /// <summary>
        /// 获取请求token的秘钥
        /// </summary>
        /// <returns></returns>
        public static string GetRequestKey() => RequestKey;
        /// <summary>
        /// 获取刷新token的秘钥
        /// </summary>
        /// <returns></returns>
        public static string GetRefreshKey() => RefreshKey;
        public static long ToUnixEpochDate(DateTime date) =>
            (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        /// <summary>
        /// 生成jwt令牌
        /// </summary>
        /// <param name="key">秘钥</param>
        /// <param name="payLoad">载荷</param>
        /// <param name="expiresMinute">过期时间</param>
        /// <param name="header">JWT的header一般不填</param>
        /// <returns></returns>
        public static string CreateToken(string key,Dictionary<string, object> payLoad = null, int expiresMinute = 2, Dictionary<string, object> header = null)
        {
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
            var encodedSignature = CreateEncodedSignature(key,encodedHeader,encodedPayload);
            var encodedJwt = string.Concat(encodedHeader, ".", encodedPayload, ".", encodedSignature);
            return encodedJwt;
        }
        /// <summary>
        /// 验证jwt令牌
        /// </summary>
        /// <param name="key">秘钥</param>
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
        public static Tuple<bool, ResponseType> ValidateJwt(string key,string str)
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
                if (Type.ToUpper() == "HS256")
                {
                    var encodedSignature = CreateEncodedSignature(key, JwtData.Item1, JwtData.Item2);
                    //var encodedSignature1 = Convert.ToBase64String(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(First, ",", Second))));
                    flag = flag && (JwtData.Item3 == encodedSignature);
                    if (flag)//自定义验证全部写在这里
                    {
                        var now = ToUnixEpochDate(DateTime.UtcNow);
                        long exp = JSecond.Value<long>("exp");
                        if (now > exp)
                        {
                            return Tuple.Create(false, ResponseType.Overdue);

                        }
                        return Tuple.Create(flag, ResponseType.Success);
                    }
                    else
                    {
                        return Tuple.Create(flag, ResponseType.IllegalToken);
                    }
                }
                else
                {
                    return Tuple.Create(false, ResponseType.UnkonwType);
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
        public static Tuple<string,string,string> GetJwtInfo(string Jwt)
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
        /// <summary>
        /// 根据jwt的前两段内容生成jwt签名段
        /// </summary>
        /// <param name="key">加密秘钥</param>
        /// <param name="encodedHeader">baseURl64之后的第一段内容</param>
        /// <param name="encodedPayload">baseURl64之后的第二段内容</param>
        /// <returns></returns>
        public static string CreateEncodedSignature(string key,string encodedHeader, string encodedPayload)
        {
            var hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(key));
            var encodedSignature = Base64UrlEncoder.Encode(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(encodedHeader, ".", encodedPayload))));
            return encodedSignature;
        }
        /// <summary>
        /// 根据原先的token重新生成一个过期时间不同的token
        /// </summary>
        /// <param name="key">秘钥</param>
        /// <param name="JwtData">JWT数据</param>
        /// <param name="OtherPayload">其他需要修改的数据</param>
        /// <returns></returns>
        public static string CreateNewToken(string key, Tuple<string, string, string> JwtData,int expiresMinute, Dictionary<string,object> OtherPayload=null)
        {
            string encodedHeader = JwtData.Item1;
            string encodedPayload = JwtData.Item2;
            //获取用户信息模型
            var info = JObject.Parse(Base64UrlEncoder.Decode(encodedPayload));
            var now = DateTime.UtcNow;
            info["nbf"] = ToUnixEpochDate(now);//可用时间起始
            info["exp"] = ToUnixEpochDate(now.Add(TimeSpan.FromMinutes(expiresMinute)));//可用时间结束
            if (OtherPayload != null)
            {
                foreach(string PayLoadKey in OtherPayload.Keys)
                {
                    info[PayLoadKey] = JsonConvert.SerializeObject(OtherPayload[PayLoadKey]);
                }
            }
            encodedPayload = Base64UrlEncoder.Encode(JsonConvert.SerializeObject(info));
            return string.Concat(encodedHeader, ".", encodedPayload, ".", CreateEncodedSignature(key, encodedHeader, encodedPayload));

        }

        private static void GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json",optional: true, reloadOnChange: true);
            Configuration = builder.Build();
            RequestKey = Configuration.GetSection("RequestSecurityKey").Value.Trim();
            RefreshKey= Configuration.GetSection("RefreshSecurityKey").Value.Trim();
        }
        public enum ResponseType
        {
            [Description("查询数据成功")]
            GetInfoSucess = 2000,
            [Description("操作成功")]
            OperationSucess = 2001,
            [Description("操作失败")]
            OperationFail = 2002,
            [Description("出现异常!")]
            Exception = -1,
            [Description("令牌验证通过")]
            Success,
            [Description("非读取到令牌")]
            NoToken= 50014,
            [Description("非法令牌")]
            IllegalToken = 50015,
            [Description("令牌已经过期！")]
            Overdue = 50016,
            [Description("未定义的验证类型！")]
            UnkonwType = 50017,
            [Description("登录成功")]
            LoginSucess = 50000,
            [Description("账号不存在！")]
            AccountNotExists = 50007,
            [Description("密码错误")]
            PwdError = 50008,
            [Description("账号已经存在")]
            AccountAlreadyExists = 50009,
            [Description("获取Token成功")]
            TokenSucess = 60000,

        }
    }
}
