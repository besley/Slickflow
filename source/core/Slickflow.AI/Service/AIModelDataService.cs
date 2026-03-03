using Slickflow.AI.Configuration;
using Slickflow.AI.Entity;
using Slickflow.AI.Manager;
using Slickflow.Data;

namespace Slickflow.AI.Service
{
    public class AiModelDataService : IAiModelDataService
    {
        #region AIModel Data Service
        public AiModelProviderEntity GetModelById(int id)
        {
            var modelManager = new AiModelProviderManager();
            var entity = modelManager.GetById(id);

            return entity;
        }

        public List<AiModelProviderEntity> GetModelListAll()
        {
            var modelManager = new AiModelProviderManager();
            var list = modelManager.GetAll();

            return list;
        }

        public List<AiModelProviderEntity> GetModelListByType(string modelType)
        {
            var modelManager = new AiModelProviderManager();
            return modelManager.GetByModelType(modelType ?? string.Empty);
        }

        public int CreateModel(AiModelProviderEntity modelEntity)
        {
            var newModelId = 0;
            var modelManager = new AiModelProviderManager();
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                //insert the entity
                modelEntity.ApiUUID = Guid.NewGuid().ToString();
                var secrentKey = ApiKeyCryptoHelper.Encrypt(modelEntity.ApiKey, modelEntity.ApiUUID);
                modelEntity.ApiKey = secrentKey;     //store secrentKey
                modelEntity.CreatedDateTime = DateTime.UtcNow;

                newModelId = modelManager.Insert(session.Connection, modelEntity, session.Transaction);
                session.Commit();

                return newModelId;
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

        public Boolean UpdateModel(AiModelProviderEntity modelEntity)
        {
            var modelManager = new AiModelProviderManager();
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                //update the entity
                var existedModelEntity = modelManager.GetById(session.Connection, modelEntity.Id, session.Transaction);
                if (modelEntity.ApiKey != existedModelEntity.ApiKey)
                {
                    modelEntity.ApiKey = ApiKeyCryptoHelper.Encrypt(modelEntity.ApiKey, modelEntity.ApiUUID);
                }
                modelEntity.UpdatedDateTime = DateTime.UtcNow;
                modelManager.Update(session.Connection, modelEntity, session.Transaction);
                session.Commit();
                return true;
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
            return false;
        }

        public void DeleteModel(int modelId)
        {
            var modelManager = new AiModelProviderManager();
            modelManager.DeleteAgent(modelId);
        }
        #endregion

        #region AxConfig Data Service
        public AiActivityConfigEntity GetAiActivityConfigByProcessVersionActivity(string processId, string version, string activityId)
        {
            var axConfigManager = new AiActivityConfigManager();
            var axConfigEntity = axConfigManager.GetByProcessVersionActivity(processId, version, activityId);
            return axConfigEntity;
        }

        /// <summary>
        /// Save AxConfig and Variables
        /// 保存AI节点配置和变量信息
        /// </summary>
        public void SaveAiActivityConfigInfo(AiActivityConfigEntity axConfigEntity)
        {
            var axConfigManager = new AiActivityConfigManager();
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                // 检查是否存在相同 process_id, version, activity_id 的记录
                var existingAxConfigEntity = axConfigManager.GetByProcessVersionActivity(session.Connection, axConfigEntity.ProcessId, axConfigEntity.Version, axConfigEntity.ActivityId, session.Transaction);

                if (existingAxConfigEntity == null)
                {
                    // 2.1 当没有相同组合键的记录时，直接保存配置记录和配置变量记录
                    axConfigEntity.CreatedDateTime = DateTime.UtcNow;
                    int newAxConfigId = axConfigManager.Insert(session.Connection, axConfigEntity, session.Transaction);
                }
                else
                {
                    // 2.2 当有相同组合键的记录时，对主表 AxConfig 表做更新，并对变量表里面的记录也做插入或者更新
                    axConfigEntity.Id = existingAxConfigEntity.Id;
                    axConfigEntity.UpdatedDateTime = DateTime.UtcNow;
                    axConfigManager.Update(session.Connection, axConfigEntity, session.Transaction);
                }

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

        public void DeleteAiActivityConfig(string processId, string version, string activityId)
        {
            var axConfigManager = new AiActivityConfigManager();
            axConfigManager.Delete(processId, version, activityId);
        }
        #endregion
    }
}
