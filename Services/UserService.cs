using IRepository;
using IServices;
using IServices.ResModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Utils;
using static Utils.JwtHelper;

namespace Services
{
    public class UserService :BaseService<userinfo>, IUserService
    {
        private IUserRepository userRepository;
        private IConfiguration Configuration;
        public UserService(IUserRepository userRepository, IConfiguration Configuration) :base(userRepository)
        {
            this.userRepository = userRepository;
            this.Configuration = Configuration;
        }

        public ResponseModel ExportUserinfo(Expression<Func<userinfo, bool>> WhereCondition)
        {
            throw new NotImplementedException();
        }

        public ResponseModel ImportUserinfo(Stream s)
        {
            throw new NotImplementedException();
        }
        public ResponseModel Login(string account, string password)
        {
            string PwdKey = Configuration.GetSection("AESKey").Value;
            string RequestKey = GetRequestKey();
            string RefreshKey = GetRefreshKey();
            ResponseModel res = new ResponseModel();
            var userlist = userRepository.GetUserInfo((a, b, c) => a.UserAccount == account);
            if (userlist.Count == 0)
            {
                res.code = (int)ResponseType.AccountNotExists;
                res.message = ReflectionConvertHelper.GetEnumDescription(ResponseType.AccountNotExists);
            }
            else
            {
                string AESPwd = AESHelper.AesEncrypt(password, PwdKey);
                var userinfo = userlist.Where(t => t.UserPassWord == AESPwd).FirstOrDefault();
                if (userinfo != null)
                {
                    res.code = (int)ResponseType.LoginSucess;
                    res.message = ReflectionConvertHelper.GetEnumDescription(ResponseType.LoginSucess);
                    var userinfodic = ReflectionConvertHelper.ConvertObjectToDictionary(userinfo);
                    //生成请求token 测试用1分钟
                    string RequestToken = CreateToken(RequestKey, userinfodic,10);
                    //生成RefreshToken
                    //RefreshToken的有效期，这里放置了一天,需要改几天就乘几
                    int LimitTime = 24 * 60;
                    string RefreshToken = CreateToken(RefreshKey, userinfodic, LimitTime);
                    res.items = new { RequestToken = RequestToken, RefreshToken = RefreshToken };
                }
                else
                {
                    res.code = (int)ResponseType.PwdError;
                    res.message = ReflectionConvertHelper.GetEnumDescription(ResponseType.PwdError);
                }
            }
            return res;

        }

        public ResponseModel RefreshToken(JObject value)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                string RefreshToken = value.Value<string>("refreshToken");
                if (RefreshToken == null) 
                {
                    res.code = (int)ResponseType.NoToken;
                    res.message = ReflectionConvertHelper.GetEnumDescription(ResponseType.NoToken);
                    return res;
                }
                string RequestKey = GetRequestKey();
                string RefreshKey = GetRefreshKey();
                //验证token有效性
                var ValidateData = ValidateJwt(RefreshKey, RefreshToken);
                //验证失败
                if (!ValidateData.Item1)
                {
                    res.code = (int)ValidateData.Item2;
                    res.message = ReflectionConvertHelper.GetEnumDescription(ValidateData.Item2);
                }
                else
                {
                    //验证成功
                    //jwt先拆成3段
                    var JwtData = GetJwtInfo(RefreshToken);
                    //jwt前两段重新用requestkey加密
                    string RequestToken = CreateEncodedSignature(RequestKey, JwtData.Item1, JwtData.Item2);
                    res.code = (int)ResponseType.TokenSucess;
                    res.message = ReflectionConvertHelper.GetEnumDescription(ResponseType.TokenSucess);
                    res.items = CreateNewToken(RequestKey, JwtData, 10);
                }               
            }
            catch(Exception e)
            {
                res.code = (int)ResponseType.Exception;
                res.message = e.Message;
            }
            return res;
        }
    }
}
