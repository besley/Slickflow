using System;
using System.Collections.Generic;
using System.Linq;
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;


namespace Slickflow.WebTest.Models
{
    /// <summary>
    /// 流程模型模拟
    /// </summary>
    internal static class ProcessModelMimic
    {
        #region 用户列表初始化
        /// <summary>
        /// 用户列表
        /// </summary>
        internal static readonly IDictionary<string, string> _toDoUserList = new Dictionary<string, string>();

        private static void Initialize()
        {
            _toDoUserList.Clear();

            _toDoUserList.Add("10", "Alice(模拟)");
            _toDoUserList.Add("11", "Andrew(模拟)");
            _toDoUserList.Add("20", "Bill(模拟)");
            _toDoUserList.Add("21", "Betty(模拟)");
            _toDoUserList.Add("30", "Colinton(模拟)");
            _toDoUserList.Add("31", "Cindy(模拟)");
            _toDoUserList.Add("40", "Daney(模拟)");
            _toDoUserList.Add("41", "Donald(模拟)");
            _toDoUserList.Add("50", "Eric(模拟)");
            _toDoUserList.Add("51", "Eimmon(模拟)");
            _toDoUserList.Add("60", "Fibbe(模拟)");
            _toDoUserList.Add("61", "Frank(模拟)");
            _toDoUserList.Add("70", "George(模拟)");
            _toDoUserList.Add("71", "Glant(模拟)");
            _toDoUserList.Add("80", "Honywell(模拟)");
            _toDoUserList.Add("81", "Heydison(模拟)");
            _toDoUserList.Add("90", "Iran(模拟)");
            _toDoUserList.Add("91", "Ian(模拟)");
            _toDoUserList.Add("100", "Jack(模拟)");
            _toDoUserList.Add("101", "Jenny(模拟)");
            _toDoUserList.Add("110", "Kate(模拟)");
            _toDoUserList.Add("111", "King(模拟)");
            _toDoUserList.Add("120", "Lary(模拟)");
            _toDoUserList.Add("121", "Lapad(模拟)");
            _toDoUserList.Add("130", "Monica(模拟)");
            _toDoUserList.Add("131", "Mole(模拟)");
            _toDoUserList.Add("140", "Newton(模拟)");
            _toDoUserList.Add("141", "Nike(模拟)");
            _toDoUserList.Add("150", "Oschard(模拟)");
            _toDoUserList.Add("151", "Oliven(模拟)");
            _toDoUserList.Add("160", "Peter(模拟)");
            _toDoUserList.Add("161", "Paul(模拟)");
            _toDoUserList.Add("170", "QQ(模拟)");
            _toDoUserList.Add("171", "Queen(模拟)");
            _toDoUserList.Add("180", "Raquel(模拟)");
            _toDoUserList.Add("181", "Rubby(模拟)");
            _toDoUserList.Add("190", "Sabrina(模拟)");
            _toDoUserList.Add("191", "Steven(模拟)");
            _toDoUserList.Add("200", "Ted(模拟)");
            _toDoUserList.Add("201", "Terrisa(模拟)");
            _toDoUserList.Add("210", "Uriane(模拟)");
            _toDoUserList.Add("211", "Universe(模拟)");
            _toDoUserList.Add("220", "Venica(模拟)");
            _toDoUserList.Add("221", "Vectoria(模拟)");
            _toDoUserList.Add("230", "White(模拟)");
            _toDoUserList.Add("231", "Wesley(模拟)");
            _toDoUserList.Add("240", "Ximan(模拟)");
            _toDoUserList.Add("241", "Xiusey(模拟)");
            _toDoUserList.Add("250", "Yonga(模拟)");
            _toDoUserList.Add("251", "Yukkiy(模拟)");
            _toDoUserList.Add("260", "Zick(模拟)");
            _toDoUserList.Add("261", "Zetophy(模拟)");
        }
        #endregion

        /// <summary>
        /// 追加模拟测试用户
        /// </summary>
        /// <param name="nextSteps">下一步列表</param>
        /// <param name="runner">当前执行用户</param>
        /// <returns>下一步列表</returns>
        internal static IList<NodeView> AppendMimicUser(List<NodeView> nextSteps, WfAppRunner runner)
        {
            Initialize();

            foreach (var step in nextSteps)
            {
                if (step.Users == null || step.Users.Count == 0)
                {
                    if (step.ActivityType != ActivityTypeEnum.EndNode)
                    {
                        step.Users = GetUserListRandom(runner.UserID);
                    }
                }
            }
            return nextSteps;
        }

        /// <summary>
        /// 加载预选步骤人员列表
        /// </summary>
        /// <param name="nextSteps">下一步步骤活动</param>
        /// <param name="nextActivityPerformers">预选人员列表</param>
        /// <param name="clearFirst">是否清除</param>
        /// <returns>步骤列表</returns>
        internal static List<NodeView> AppendPremilinaryUser(List<NodeView> nextSteps,
            IDictionary<string, PerformerList> nextActivityPerformers,
            bool clearFirst)
        {
            foreach (var p in nextActivityPerformers)
            {
                var activityGUID = p.Key;
                foreach (var s in nextSteps)
                {
                    if (s.ActivityGUID == activityGUID)
                    {
                        if (clearFirst == true && s.Users != null) s.Users.Clear();
                        //添加预选用户
                        foreach (var performer in p.Value)
                        {
                            if (s.Users == null) s.Users = new List<User>();
                            s.Users.Add(new User { UserID = performer.UserID, UserName = performer.UserName });
                        }
                    }
                }
            }
            return nextSteps;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns>用户对象</returns>
        internal static IList<User> GetUserListRandom(string userID)
        {
            var random = new Random();
            var users = new List<User>();

            _toDoUserList.Remove(userID);

            //模拟用户一
            var entry = _toDoUserList.ElementAt(random.Next(0, _toDoUserList.Count()));
            var user = new User { UserID = entry.Key, UserName = entry.Value };
            users.Add(user);
            _toDoUserList.Remove(user.UserID);

            //模拟用户二
            entry = _toDoUserList.ElementAt(random.Next(0, _toDoUserList.Count()));
            user = new User { UserID = entry.Key, UserName = entry.Value };
            users.Add(user);
            _toDoUserList.Remove(user.UserID);

            //模拟用户三
            entry = _toDoUserList.ElementAt(random.Next(0, _toDoUserList.Count()));
            user = new User { UserID = entry.Key, UserName = entry.Value };
            users.Add(user);
            _toDoUserList.Remove(user.UserID);

            return users;
        }
    }
}