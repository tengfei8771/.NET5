using Entity.Models;
using IRepository;
using SqlSugarAndEntity;
using System.Data;
using System.Linq;

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
        
    }
}
