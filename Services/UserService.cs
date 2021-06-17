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
        private IOrgService orgService;
        public UserService(IUserRepository userRepository, IOrgService orgService, IConfiguration Configuration) :base(userRepository)
        {
            this.userRepository = userRepository;
            this.orgService = orgService;
            this.Configuration = Configuration;
        }

        public ResponseModel CreateUserInfo(userinfo user, List<usermaporg> map)
        {
            int total = 0;
            if (userRepository.GetUserInfo((a, b, c) => a.UserAccount == user.UserAccount, 1, 10, ref total).FirstOrDefault() != null)
            {
                return new ResponseModel()
                {
                    code = (int)ResponseTypeEnum.AccountAlreadyExists,
                    message = ReflectionConvertHelper.GetEnumDescription(ResponseTypeEnum.AccountAlreadyExists)
                };
            }

            return CreateResponseModel(userRepository.InsertMany, user, map);
        }

        public ResponseModel DeleteUserInfo(Expression<Func<userinfo, bool>> UserExp, Expression<Func<usermaporg, bool>> MapExp)
        {
            return CreateResponseModel(userRepository.DeleteUserinfo, UserExp, MapExp);
        }

        public ResponseModel ExportUserinfo(Expression<Func<userinfo, bool>> WhereCondition)
        {
            throw new NotImplementedException();
        }
        public ResponseModel GetUserInfo(string Name, string UserAccount, string UserPhone, string IdNumber, string OrgName, int page, int limit)
        {
            string PwdKey = Configuration.GetSection("AESKey").Value;
            ResponseModel res = new ResponseModel();
            try
            {
                int total = 0;
                Expression<Func<userinfo, usermaporg, orginfo, bool>> OrginExp = null;
                Expression<Func<userinfo, usermaporg, orginfo, bool>> WhereExp = OrginExp
                    .AndIF(!string.IsNullOrEmpty(Name), (a, b, c) => a.UserName.Contains(Name))
                    .AndIF(!string.IsNullOrEmpty(UserAccount), (a, b, c) => a.UserPhone.Contains(UserPhone))
                    .AndIF(!string.IsNullOrEmpty(IdNumber), (a, b, c) => a.IdNumber.Contains(IdNumber))
                    .AndIF(!string.IsNullOrEmpty(OrgName), (a, b, c) => c.OrgName.Contains(OrgName));
                var list = userRepository.GetUserInfo(WhereExp, page, limit, ref total);
                list.ForEach(t =>
                {
                    //把用户密码解密
                    t.UserPassWord = AESHelper.AesDecrypt(t.UserPassWord, PwdKey);
                });
                res.code = (int)ResponseTypeEnum.GetInfoSucess;
                res.message = ReflectionConvertHelper.GetEnumDescription(ResponseTypeEnum.GetInfoSucess);
                res.total = total;
                res.items = list;
            }
            catch(Exception e)
            {
                res.code= (int)ResponseTypeEnum.Exception;
                res.message = e.Message;
            }
            return res; 
            //return CreateResponseModelByPage(userRepository.GetUserInfo, WhereExp, page, limit, ref total);
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
            int total = 0;
            var userlist = userRepository.GetUserInfo((a, b, c) => a.UserAccount == account,1,10,ref total);
            if (userlist.Count == 0)
            {
                res.code = (int)ResponseTypeEnum.AccountNotExists;
                res.message = ReflectionConvertHelper.GetEnumDescription(ResponseTypeEnum.AccountNotExists);
            }
            else
            {
                string AESPwd = AESHelper.AesEncrypt(password, PwdKey);
                var userinfo = userlist.Where(t => t.UserPassWord == AESPwd).FirstOrDefault();
                if (userinfo != null)
                {
                    res.code = (int)ResponseTypeEnum.LoginSucess;
                    res.message = ReflectionConvertHelper.GetEnumDescription(ResponseTypeEnum.LoginSucess);
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
                    res.code = (int)ResponseTypeEnum.PwdError;
                    res.message = ReflectionConvertHelper.GetEnumDescription(ResponseTypeEnum.PwdError);
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
                    res.code = (int)ResponseTypeEnum.NoToken;
                    res.message = ReflectionConvertHelper.GetEnumDescription(ResponseTypeEnum.NoToken);
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
                    res.code = (int)ResponseTypeEnum.TokenSucess;
                    res.message = ReflectionConvertHelper.GetEnumDescription(ResponseTypeEnum.TokenSucess);
                    res.items = CreateNewToken(RequestKey, JwtData, 10);
                }               
            }
            catch(Exception e)
            {
                res.code = (int)ResponseTypeEnum.Exception;
                res.message = e.Message;
            }
            return res;
        }

        public ResponseModel UpdateUserInfo(userinfo user, List<usermaporg> map)
        {
            return CreateResponseModel(userRepository.UpdateUserInfo, user, map);
        }
        public ResponseModel GetRoleAuthorized(decimal roleId, int page, int limit)
        {
            int total = 0;
            return CreateResponseModelByPage(userRepository.GetRoleAuthorized, roleId, page, limit, ref total);
        }

        public ResponseModel GetRoleNotAuthorized(decimal roleId, int page, int limit)
        {
            int total = 0;
            return CreateResponseModelByPage(userRepository.GetRoleNotAuthorized, roleId, page, limit, ref total);
        }
    }
}
