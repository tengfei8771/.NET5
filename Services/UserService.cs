using IRepository;
using IServices;
using IServices.ResModel;
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;

namespace Services
{
    public class UserService :BaseService<userinfo>, IUserService
    {
        private IUserRepository userRepository;
        public UserService(IUserRepository userRepository):base(userRepository)
        {
            this.userRepository = userRepository;
        }

        public ResponseModel ExportUserinfo(Expression<Func<userinfo, bool>> WhereCondition)
        {
            throw new NotImplementedException();
        }

        public ResponseModel ImportUserinfo(Stream s)
        {
            throw new NotImplementedException();
        }
    }
}
