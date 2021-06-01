using IRepository;
using IServices;
using IServices.ResModel;
using Microsoft.Extensions.Configuration;
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Utils;

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
            string key = Configuration.GetSection("AESKey").Value;
            ResponseModel res = new ResponseModel();
            var userlist = userRepository.GetUserInfo((a, b, c) => a.UserAccount == account);
            if (userlist.Count == 0)
            {
                res.code = 50007;
                res.message = "账号不存在!";
            }
            else
            {
                string AESPwd = AESHelper.AesEncrypt(password, key);
                var userinfo = userlist.Where(t => t.UserPassWord == AESPwd).FirstOrDefault();
                if (userinfo != null)
                {
                    res.code = 2000;
                    res.message = "登录成功";
                    var userinfodic = ReflectionConvertHelper.ConvertObjectToDictionary(userinfo);
                    res.items = JwtHelper.CreateToken(userinfodic, 30);
                }
                else
                {
                    res.code = 50008;
                    res.message = "密码错误!";
                }
            }
            return res;

        }
    }
}
