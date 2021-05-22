using IRepository;
using IServices;
using IServices.ResModel;
using Microsoft.Extensions.Configuration;
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Services
{
    public class LoginService : BaseService<userinfo>, ILoginService
    {
        private ILoginRepository loginRepository;
        private IConfiguration Configuration;
        public LoginService(ILoginRepository loginRepository, IConfiguration Configuration):base(loginRepository)
        {
            this.loginRepository = loginRepository;
            this.Configuration = Configuration;
        }
        public ResponseModel Login(string account, string password)
        {
            string key = Configuration.GetSection("AESKey").Value;
            ResponseModel res = new ResponseModel();
            var userlist = loginRepository.GetLoginUserInfo(account);
            if (userlist.Count == 0)
            {
                res.Code = 50007;
                res.Message = "账号不存在!";
            }
            else
            {
                string AESPwd = AESHelper.AesEncrypt(password,key);
                var userinfo = userlist.Where(t => t.UserPassWord == AESPwd).FirstOrDefault();
                if (userinfo != null)
                {
                    res.Code = 50000;
                    res.Message = "登录成功";
                }
                else
                {
                    res.Code = 50008;
                    res.Message = "密码错误!";
                }
            }
            return res;
            
        }
    }
}
