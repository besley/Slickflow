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
        int CreateModel(AiModelProviderEntity modelEntity);
        Boolean UpdateModel(AiModelProviderEntity modelEntity);
        void DeleteModel(int modelId);
        AiActivityConfigEntity GetAiActivityConfigByUUID(string uuid);
        void SaveAiActivityConfigInfo(AiActivityConfigEntity axConfigEntity);
        void DeleteAiActivityConfig(string configUUID);
    }
}
