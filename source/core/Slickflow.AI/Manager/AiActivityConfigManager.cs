using Dapper;
using Slickflow.Data;
using Slickflow.AI.Entity;
using System;
using System.Data;


namespace Slickflow.AI.Manager
{
    /// <summary>
    /// AxConfig Manager
    /// AI节点配置管理类
    /// </summary>
    public class AiActivityConfigManager : ManagerBase
    {
        #region  axconfig manager getdata 获取AI节点配置数据
        public AiActivityConfigEntity GetById(int id)
        {
            var entity = Repository.GetById<AiActivityConfigEntity>(id);
            return entity;
        }

        public AiActivityConfigEntity GetById(IDbConnection conn, int id, IDbTransaction trans)
        {
            var entity = Repository.GetById<AiActivityConfigEntity>(conn, id, trans);
            return entity;
        }

        public AiActivityConfigEntity GetByUUID(string configUUID)
        {
            AiActivityConfigEntity entity = null;
            var list = Repository.GetAll<AiActivityConfigEntity>()
                .Where<AiActivityConfigEntity>(
                    p => p.ConfigUUID == configUUID)
                .ToList();

            if (list.Count() == 1)
            {
                entity = list[0];
            }
            return entity;
        }

        public AiActivityConfigEntity GetByUUID(IDbConnection conn, string configUUID, IDbTransaction trans)
        {
            AiActivityConfigEntity entity = null;
            var list = Repository.GetAll<AiActivityConfigEntity>(conn, trans)
                .Where<AiActivityConfigEntity>(
                    p => p.ConfigUUID == configUUID)
                .ToList();

            if (list.Count() == 1)
            {
                entity = list[0];
            }
            return entity;
        }

        public List<AiActivityConfigEntity> GetAll()
        {
            var list = Repository.GetAll<AiActivityConfigEntity>().ToList<AiActivityConfigEntity>();
            return list;
        }
        #endregion

        #region Addition, deletion, modification, and search of axconfig records 新增、更新和删除AI节点配置数据
        public int Insert(AiActivityConfigEntity entity)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                int agentId = Insert(session.Connection, entity, session.Transaction);
                session.Commit();

                return agentId;
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

        public int Insert(IDbConnection conn, AiActivityConfigEntity entity, IDbTransaction trans)
        {
            var entityExisted = GetByUUID(conn, entity.ConfigUUID, trans);
            if (entityExisted != null)
            {
                throw new ApplicationException("AxConfigManager.Insert: A service record with the same name already exists!");
            }

            // 验证必填字段
            if (!entity.ModelProviderId.HasValue)
            {
                throw new ApplicationException("AxConfigManager.Insert: ModelProviderId is required and cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(entity.ModelName))
            {
                throw new ApplicationException("AxConfigManager.Insert: ModelName is required and cannot be null or empty.");
            }

            entity.CreatedDateTime = DateTime.UtcNow;
            return Repository.Insert<AiActivityConfigEntity>(conn, entity, trans);
        }

        

        /// <summary>
        /// Update AxConfig
        /// 更新AI节点配置记录
        /// </summary>
        /// <param name="entity"></param>
        public void Update(AiActivityConfigEntity entity)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                entity.UpdatedDateTime = DateTime.UtcNow;
                Repository.Update<AiActivityConfigEntity>(session.Connection, entity, session.Transaction);

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

        public void Update(IDbConnection conn, AiActivityConfigEntity entity, IDbTransaction trans)
        {
            // 验证必填字段
            if (!entity.ModelProviderId.HasValue)
            {
                throw new ApplicationException("AxConfigManager.Update: ModelProviderId is required and cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(entity.ModelName))
            {
                throw new ApplicationException("AxConfigManager.Update: ModelName is required and cannot be null or empty.");
            }

            Repository.Update<AiActivityConfigEntity>(conn, entity, trans);
        }

       

        /// <summary>
        /// Delete AxConfig
        /// 删除AI节点配置记录
        /// </summary>
        public void Delete(string configUUID)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                //delete axconfig data
                string strSql = @"DELETE FROM ai_activity_config
                                WHERE config_uuid=@configUUID";

                Repository.Execute(session.Connection, strSql,
                    new { configUUID = configUUID },
                    session.Transaction);

                session.Commit();
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
        #endregion
    }
}
