using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Slickflow.WebDemo.Common;
using Slickflow.Engine.Common;
using Slickflow.WebDemo.Business;
using Slickflow.WebDemo.Entity;
using System.Text.RegularExpressions;

namespace Slickflow.WebDemo
{
    public class BasePage : System.Web.UI.Page
    {

        #region 全局定义
        /// <summary>
        /// 总经理
        /// </summary>
        public readonly int GeneralManager = 8;

        /// <summary>
        /// 副总经理
        /// </summary>
        public readonly int DeputyGeneralManager = 7;

        /// <summary>
        /// 主管总监
        /// </summary>
        public readonly int Director = 4;

        /// <summary>
        /// 人事经理
        /// </summary>
        public readonly int HRManager = 3;

        /// <summary>
        /// 部门经理
        /// </summary>
        public readonly int DepManager = 2;

        /// <summary>
        /// 普通员工
        /// </summary>
        public readonly int Employees = 1;

        #endregion


        #region 登录信息

        /// <summary>
        /// 获取登录用户ID
        /// </summary>
        public int LoginUserID
        {
            get
            {
                try
                {
                    if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["UserId"] != null)
                    {
                        return Helper.ConverToInt32(HttpContext.Current.Session["UserId"].ToString());
                    }
                }
                catch (Exception ex) { }
                return 0;
            }
        }

        /// <summary>
        /// 获取登录用户姓名
        /// </summary>
        public string LoginUserName
        {
            get
            {
                try
                {
                    if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["UserName"] != null)
                    {
                        return HttpContext.Current.Session["UserName"].ToString();
                    }
                }
                catch (Exception ex) { }
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取登录用户角色ID
        /// </summary>
        public int LoginRoleID
        {
            get
            {
                try
                {
                    if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["RoleId"] != null)
                    {
                        return Helper.ConverToInt32(HttpContext.Current.Session["RoleId"].ToString());
                    }
                }
                catch (Exception ex) { }
                return 0;
            }
        }

        /// <summary>
        /// 获取登录用户角色名称
        /// </summary>
        public string LoginRoleName
        {
            get
            {
                try
                {
                    if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["RoleName"] != null)
                    {
                        return HttpContext.Current.Session["RoleName"].ToString();
                    }
                }
                catch (Exception ex) { }
                return string.Empty;
            }
        }
        #endregion

        /// <summary>
        /// 构造流转条件
        /// </summary>
        /// <param name="conditions">条件字符串</param>
        /// <returns></returns>
        protected Dictionary<string, string> GetCondition(string conditions)
        {
            //根据条件取得下一步骤
            Dictionary<string, string> condition = new Dictionary<string, string>();
            condition.Add("RoleID", LoginRoleID.ToString());//角色ID

            if (!string.IsNullOrEmpty(conditions))
            {
                string[] conditionItemArray = conditions.Split(',');
                foreach (string conditionItems in conditionItemArray)
                {
                    if (!string.IsNullOrEmpty(conditionItems))
                    {
                        string[] conditionItem = conditionItems.Split('-');
                        if (conditionItem.Length == 2)
                        {
                            string key = conditionItem[0];
                            string value = conditionItem[1];

                            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                            {
                                condition.Add(key, value);
                            }
                        }
                    }
                }
            }
            return condition;
        }


        private string regText = @"{0}\[(?<val>.*?)\]";
        /// <summary>
        /// 从集合中获取出值
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        private string GetValueOfNodeIdList(string idList, string prefix)
        {
            if (!string.IsNullOrEmpty(idList))
            {
                string reg = string.Format(regText, prefix);
                Match match = Regex.Match(idList, reg, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                return match.Groups["val"].Value.Trim();
            }
            return string.Empty;
        }



        public IDictionary<string, PerformerList> NextActivityPerformers(string nextActivityPerformers)
        {
            IDictionary<string, PerformerList> nextActivityPerformersDictionary = new Dictionary<string, PerformerList>();
            string[] array = nextActivityPerformers.Split(',');
            foreach (string items in array)
            {
                string stepId = GetValueOfNodeIdList(items, "step");
                if (!string.IsNullOrWhiteSpace(stepId) && stepId != "0")
                {
                    string userId = GetValueOfNodeIdList(items, "member");

                    Performer performer = null;
                    if (!string.IsNullOrWhiteSpace(userId) && userId != "0" && userId.Length > 0)
                    {
                        SysUserEntity userEntity = WorkFlows.GetSysUserModel(Convert.ToInt32(userId));
                        if (userEntity != null && userEntity.ID > 0)
                        {
                            performer = new Performer(userId, userEntity.UserName);
                        }
                    }
                    if (performer == null)
                        performer = new Performer("0", string.Empty);

                    if (nextActivityPerformersDictionary.ContainsKey(stepId))
                    {
                        (nextActivityPerformersDictionary[stepId]).Add(performer);
                    }
                    else
                    {
                        PerformerList pList = new PerformerList();
                        pList.Add(performer);
                        nextActivityPerformersDictionary.Add(stepId, pList);
                    }
                }
            }
            return nextActivityPerformersDictionary;
        }


    }
}