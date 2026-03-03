using Slickflow.AI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.AI.Service
{
    public interface IAiModelDataService
    {
        AiModelProviderEntity GetModelById(int id);
        List<AiModelProviderEntity> GetModelListAll();
        List<AiModelProviderEntity> GetModelListByType(string modelType);
        int CreateModel(AiModelProviderEntity modelEntity);
        Boolean UpdateModel(AiModelProviderEntity modelEntity);
        void DeleteModel(int modelId);
        AiActivityConfigEntity GetAiActivityConfigByProcessVersionActivity(string processId, string version, string activityId);
        void SaveAiActivityConfigInfo(AiActivityConfigEntity axConfigEntity);
        void DeleteAiActivityConfig(string processId, string version, string activityId);
    }
}
