using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Slickflow.Engine.Common
{
    public class NextStepUtility
    {
        /// <summary>
        /// 根据文本内容反序列化下一步人员列表
        /// </summary>
        /// <param name="jsonNextStep">json格式的步骤文本</param>
        /// <returns>下一步步骤对象</returns>
        public static IDictionary<string, PerformerList> DeserializeNextStepPerformers(string jsonNextStep)
        {
            IDictionary<string, PerformerList> nextSteps = null;
            if (!string.IsNullOrEmpty(jsonNextStep))
            {
                //反序列化步骤数据
                nextSteps = JsonConvert.DeserializeObject<IDictionary<string, PerformerList>>(jsonNextStep);
            }
            return nextSteps;
        }

        /// <summary>
        /// 反序列化步骤人员列表
        /// </summary>
        /// <param name="nextSteps">下一步步骤对象</param>
        /// <returns>json格式的步骤文本</returns>
        public static string SerializeNextStepPerformers(IDictionary<string, PerformerList> nextSteps)
        {
            var jsonNextSteps = string.Empty;
            if (nextSteps != null 
                && nextSteps.Count > 0 
                && nextSteps.First().Value != null 
                && nextSteps.First().Value.Count > 0)
            {
                jsonNextSteps = JsonConvert.SerializeObject(nextSteps);
            }
            return jsonNextSteps;
        }

        /// <summary>
        /// 生成下一步人员列表
        /// </summary>
        /// <param name="activityGUID">活动GUID</param>
        /// <param name="userID">用户ID</param>
        /// <param name="userName">用户名称</param>
        /// <returns>步骤列表</returns>
        public static IDictionary<string, PerformerList> CreateNextStepPerformerList(string activityGUID, 
            string userID, 
            string userName)
        {
            var performerList = PerformerBuilder.CreatePerformerList(userID, userName);
            var nextStep = new Dictionary<string, PerformerList>();
            nextStep.Add(activityGUID, performerList);
            return nextStep;
        }
    }
}
