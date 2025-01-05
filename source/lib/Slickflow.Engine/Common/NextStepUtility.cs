using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Next Step Utility
    /// 下一步工具类
    /// </summary>
    public class NextStepUtility
    {
        /// <summary>
        /// Deserialize the next personnel list based on the text content
        /// 根据文本内容反序列化下一步人员列表
        /// </summary>
        /// <param name="jsonNextStep"></param>
        /// <returns></returns>
        public static IDictionary<string, PerformerList> DeserializeNextStepPerformers(string jsonNextStep)
        {
            IDictionary<string, PerformerList> nextSteps = null;
            if (!string.IsNullOrEmpty(jsonNextStep))
            {
                //反序列化步骤数据
                //Deserialize step data
                nextSteps = JsonConvert.DeserializeObject<IDictionary<string, PerformerList>>(jsonNextStep);
            }
            return nextSteps;
        }

        /// <summary>
        /// List of personnel involved in deserialization steps
        /// 反序列化步骤人员列表
        /// </summary>
        /// <param name="nextSteps"></param>
        /// <returns></returns>
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
        /// Generate the next step performer list
        /// 生成下一步人员列表
        /// </summary>
        /// <param name="activityGUID"></param>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
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
