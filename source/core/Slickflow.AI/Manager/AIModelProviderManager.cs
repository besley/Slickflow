using Dapper;
using System;
using Slickflow.Data;
using System.Data;
using System.Linq;
using Slickflow.AI.Entity;
using Slickflow.AI.Utility;


namespace Slickflow.AI.Manager
{
    /// <summary>
    /// Agent Manager
    /// 智能体管理类
    /// </summary>
    public class AiModelProviderManager : ManagerBase
    {
        #region Obtain model data 获取模型数据
        public AiModelProviderEntity GetById(int id)
        {
            var entity = Repository.GetById<AiModelProviderEntity>(id);
            return entity;
        }

        public AiModelProviderEntity GetById(IDbConnection conn, int id, IDbTransaction trans)
        {
            var entity = Repository.GetById<AiModelProviderEntity>(conn, id, trans);
            return entity;
        }


        public List<AiModelProviderEntity> GetAll()
        {
            var list = Repository.GetAll<AiModelProviderEntity>().ToList<AiModelProviderEntity>();
            return list;
        }
        #endregion

        #region Addition, deletion, modification, and search of agent records 新增、更新和删除智能体数据
        public int Insert(AiModelProviderEntity entity)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                var modelId = Insert(session.Connection, entity, session.Transaction);
                session.Commit();

                return modelId;
            }
            catch (System.Exception ex)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        public int Insert(IDbConnection conn, AiModelProviderEntity entity, IDbTransaction trans)
        {
            //insert 
            var modelId = Repository.Insert<AiModelProviderEntity>(conn, entity, trans);
            return modelId;
        }

        

        /// <summary>
        /// Update process
        /// 更新流程记录
        /// </summary>
        /// <param name="entity"></param>
        public void Update(AiModelProviderEntity entity)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<AiModelProviderEntity>(session.Connection, entity, session.Transaction);
                session.Commit();
            }
            catch (System.Exception)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        public void Update(IDbConnection conn, AiModelProviderEntity entity, IDbTransaction trans)
        {
            entity.UpdatedDateTime = DateTime.UtcNow;
            Repository.Update<AiModelProviderEntity>(conn, entity, trans);
        }

        /// <summary>
        /// Delete process
        /// 删除流程记录
        /// </summary>
        public void DeleteAgent(int modelId)
        {
            Repository.Delete<AiModelProviderEntity>(modelId);
        }
        #endregion
    }
}
