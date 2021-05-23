using System;
using IRepository;
using SqlSugarAndEntity;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SqlSugar;

namespace Repository
{
    public class UserRepository: BaseRepository<userinfo>,IUserRepository
    {
        //private readonly AppDBContext _appDBContext;
        //public UserRepositoryEF(AppDBContext _appDBContext) : base(_appDBContext)
        //{
        //    this._appDBContext = _appDBContext;
        //}
        //public DataTable GetUserInfo()
        //{
        //    var list= _appDBContext.Set<User>().Join()
        //}
        private IBaseMethod _baseMethod;
        public UserRepository(IBaseMethod baseMethod) : base(baseMethod)
        {
            _baseMethod = baseMethod;
        }

        public List<userinfo> GetUserByRole(decimal RoleID)
        {
            var list = _baseMethod.Db()
                .Queryable<userinfo, usermaprole>((a, b)
                 => new JoinQueryInfos(
                     JoinType.Inner, a.ID == b.UserID
                     ))
                .Where((a, b) => b.RoleID == RoleID)
                .ToList();
            return list;
        }
    }
}
