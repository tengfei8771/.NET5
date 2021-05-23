using IRepository;
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RoleRepository: BaseRepository<roleinfo>, IRoleRepository
    {
        private IBaseMethod baseMethod;
        public RoleRepository(IBaseMethod baseMethod) : base(baseMethod)
        {
            this.baseMethod = baseMethod;
        }

        public void RoleForMenu(decimal RoleId,List<rolemapmenu> map)
        {
            var db = baseMethod.Db();
            try
            {
                db.Ado.BeginTran();
                db.Deleteable<rolemapmenu>().Where(t => t.RoleID == RoleId).ExecuteCommand();
                db.Insertable(map).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch(Exception e)
            {
                db.Ado.RollbackTran();
                throw new Exception(e.Message);
            }
        }

        public void RoleForUser(decimal RoleId, List<usermaprole> map)
        {
            var db = baseMethod.Db();
            try
            {
                db.Ado.BeginTran();
                db.Deleteable<usermaprole>().Where(t => t.RoleID == RoleId).ExecuteCommand();
                db.Insertable(map).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch (Exception e)
            {
                db.Ado.RollbackTran();
                throw new Exception(e.Message);
            }
        }
    }
}
