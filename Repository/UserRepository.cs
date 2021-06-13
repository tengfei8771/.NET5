using System;
using IRepository;
using SqlSugarAndEntity;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SqlSugar;
using System.Linq.Expressions;
using SqlSugarAndEntity.BusinessModel;
using Microsoft.Extensions.Configuration;
using Utils;

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
        private IConfiguration Configuration;
        public UserRepository(IBaseMethod baseMethod, IConfiguration Configuration) : base(baseMethod)
        {
            this.Configuration = Configuration;
            _baseMethod = baseMethod;
        }

        public List<UserinfoBusinessModel> GetUserByRole(decimal RoleID)
        {
            var list = _baseMethod.Db()
                .Queryable<userinfo, usermaprole, usermaporg, orginfo>((a, b, c, d)
                   => new JoinQueryInfos(
                       JoinType.Inner, a.ID == b.UserID,
                       JoinType.Left, a.ID == c.UserID,
                       JoinType.Inner, c.OrgID == d.ID
                       ))
                .Where((a, b) => b.RoleID == RoleID)
                .GroupBy((a, b, c, d) => new
                {
                    a.ID,
                    a.UserName,
                    a.UserAccount,
                    //a.UserPassWord,
                    a.UserSex,
                    a.UserPhone,
                    a.UserRole,
                    a.IdNumber,
                    a.CreateBy
                })
                .Select((a, b, c, d) => new UserinfoBusinessModel
                {
                    ID = a.ID,
                    UserName = a.UserName,
                    //a.UserAccount,
                    //a.UserPassWord,
                    UserSex = a.UserSex,
                    UserPhone = a.UserPhone,
                    UserRole=SqlFunc.IF(a.UserRole=="0").Return("管理员").ElseIF(a.UserRole=="1").Return("普通用户").End("未知"),
                    IdNumber = a.IdNumber,
                    UserCreateBy = a.CreateBy,
                    OrgName = SqlSguarExtensionMethod.GroupConcat(d.OrgName)
                })
                .ToList();
            return list;
        }

        public List<UserinfoBusinessModel> GetUserInfo(Expression<Func<userinfo, usermaporg,orginfo, bool>> WhereExp,int page,int limit,ref int total)
        {
            string PwdKey = Configuration.GetSection("AESKey").Value;
            var list = _baseMethod.Db()
                .Queryable<userinfo, usermaporg, orginfo>((a, b, c)
                   => new JoinQueryInfos(
                       JoinType.Left, a.ID == b.UserID,
                       JoinType.Left, b.OrgID == c.ID
                       ))
                .WhereIF(WhereExp!=null,WhereExp)
                .GroupBy((a, b, c) => new
                {
                    a.ID,
                    a.UserName,
                    a.UserAccount,
                    a.UserPassWord,
                    a.UserSex,
                    a.UserPhone,
                    a.UserRole,
                    a.IdNumber,
                    a.CreateBy
                })
                .Select((a, b, c) => new UserinfoBusinessModel
                {
                    ID = a.ID,
                    UserName = a.UserName,
                    UserAccount=a.UserAccount,
                    UserPassWord=a.UserPassWord,
                    UserSex = a.UserSex,
                    UserPhone = a.UserPhone,
                    UserRole = SqlFunc.IF(a.UserRole == "0").Return("管理员").ElseIF(a.UserRole == "1").Return("普通用户").End("未知"),
                    IdNumber = a.IdNumber,
                    UserCreateBy = a.CreateBy,
                    OrgName = SqlSguarExtensionMethod.GroupConcat(c.OrgName),
                })
                .Mapper((it,cache)=> 
                {
                    it.UserPassWord = AESHelper.AesDecrypt(it.UserPassWord, PwdKey);
                    List<usermaporg> mapinfos = cache.Get(t =>
                    {
                        var ids = t.Select(t => t.ID);
                        var maplist = _baseMethod.Db().Queryable<usermaporg>().Where(t => ids.Contains(t.UserID)).ToList();
                        return maplist;
                    });
                    List<orginfo> orginfos = cache.Get(t =>
                    {
                        var orgids = mapinfos.Select(t => t.OrgID).Distinct();
                        var orglist = _baseMethod.Db().Queryable<orginfo>().Where(t => orgids.Contains(t.ID)).ToList();
                        return orglist;
                    });
                    it.OrgList = orginfos.Where(t => mapinfos.Where(y => y.UserID == it.ID && t.ID == y.OrgID).Any()).ToList();
                })
                .ToPageList(page,limit,ref total);
            return list;
        }

        public void DeleteUserinfo(Expression<Func<userinfo, bool>> UserExp, Expression<Func<usermaporg, bool>> MapExp)
        {
            var db = _baseMethod.Db();
            try
            {
                db.Ado.BeginTran();
                db.Deleteable<userinfo>().Where(UserExp).ExecuteCommand();
                db.Deleteable<usermaporg>().Where(MapExp).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch(Exception e)
            {
                db.Ado.RollbackTran();
                throw new Exception(e.Message);
            }
        }
        public void UpdateUserInfo(userinfo user, List<usermaporg> map)
        {
            var db = _baseMethod.Db();
            try
            {
                db.Ado.BeginTran();
                db.Deleteable<usermaporg>().Where(t => t.UserID == user.ID).ExecuteCommand();
                db.Updateable(user).WhereColumns(t => new { t.ID }).ExecuteCommand();
                db.Insertable(map).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch(Exception e)
            {
                db.Ado.RollbackTran();
                throw new Exception(e.Message);
            }
        }
    }
}
