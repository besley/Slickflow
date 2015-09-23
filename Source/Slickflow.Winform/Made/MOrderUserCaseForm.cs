using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Core;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Service;

namespace Slickflow.Winform
{
    /// <summary>
    /// 流程订单的模拟测试
    /// 1） 执行人员身份信息在本地构造
    /// 2） 读取角色信息
    /// </summary>
    public partial class MOrderUserCaseForm : Form
    {
        public delegate void RefreshTextDelegate(string data);

        private string application_instance_id = "10242";
        private string application_name = "WfOrder测试数据";
        private string process_guid = "5C5041FC-AB7F-46C0-85A5-6250C3AEA375";

        private static IDictionary<string, string> per_dict = null;

        public MOrderUserCaseForm()
        {
            InitializeComponent();

            //初始化人员信息
            per_dict = PerformerResource.Initialize();
        }

        /// <summary>
        /// 打单节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintOrder_Click(object sender, EventArgs e)
        {
            WfAppRunner appRunner = new WfAppRunner();
            appRunner.ProcessGUID = process_guid;
            appRunner.AppInstanceID = application_instance_id;
            appRunner.AppName = application_name;
            appRunner.UserID = per_dict["yiran"];
            appRunner.UserName = "yiran";

            //先启动流程
            var wfService = new WorkflowService();
            var r1 = wfService.StartProcess(appRunner);

            var msg = string.Format("生产订单流程开始：{0}, {1}\r\n", r1.Status, r1.Message);
            WriteText(msg);

            if (r1.Status == WfExecutedStatus.Success)
            {
                //打单环节加入条件，运行流程
                var cond = new Dictionary<string, string>();

                cond["CanUseStock"] = chkStock.Checked.ToString().ToLower();
                cond["IsHavingWeight"] = chkWeight.Checked.ToString().ToLower();

                appRunner.Conditions = cond;

                string message = string.Empty;
                var nextSteps = wfService.GetNextActivityTree(appRunner, cond);
                if (nextSteps != null)
                {
                    appRunner.NextActivityPerformers = CreateNextActivityPerformers(nextSteps);

                    var r2 = wfService.RunProcessApp(appRunner);
                    message = r2.Message;
                    WriteText(string.Format("执行【打单】: {0}, {1}\r\n", r2.Status, r2.Message));

                    if (r2.Status == WfExecutedStatus.Success)
                    {
                        WriteText(string.Format("任务已经发送到下一节点:{0}\r\n\r\n", nextSteps[0].ActivityName));
                    }
                }
                else
                {
                    message = "下一步节点不匹配！";
                    WriteText(string.Format("{0}\r\n", message));
                }
            }
        }

         /// <summary>
        /// 输出节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOutput_Click(object sender, EventArgs e)
        {
            WfAppRunner appRunner = new WfAppRunner();
            appRunner.ProcessGUID = process_guid;
            appRunner.AppInstanceID = application_instance_id;
            appRunner.AppName = application_name;
            appRunner.UserID = per_dict["andun"];
            appRunner.UserName = "andun";

            var wfService = new WorkflowService();
            var nextSteps = wfService.GetNextActivityTree(appRunner);

            if (nextSteps != null)
            {
                //构造下一步办理人的信息
                string msg2 = string.Empty;

                foreach (NodeView nodeView in nextSteps)
                {
                    //当前用例只有一个节点能够向下流转
                    appRunner.NextActivityPerformers = CreateNextActivityPerformers(nodeView);

                    if (nodeView.IsSkipTo == true)
                    {
                        var j2 = wfService.JumpProcess(appRunner);
                        msg2 = string.Format("执行【输出】并跳转:{0}, {1}\r\n", j2.Status, j2.Message);

                        WriteText(msg2);

                        if (j2.Status == WfExecutedStatus.Success)
                        {
                            WriteText(string.Format("任务已经发送到下一节点:{0}\r\n\r\n", nodeView.ActivityName));
                        }
                    }
                    else
                    {
                        var r2 = wfService.RunProcessApp(appRunner);
                        msg2 = string.Format("执行【输出】:{0}\r\n", r2.Message);

                        WriteText(msg2);

                        if (r2.Status == WfExecutedStatus.Success)
                        {
                            WriteText(string.Format("任务已经发送到下一节点:{0}\r\n", nodeView.ActivityName));
                        }
                    }
                }

            }
            else
            {
                WriteText("您当前没有办理任务!\r\n");
            }
        }

        /// <summary>
        /// 生产节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMade_Click(object sender, EventArgs e)
        {
            WfAppRunner appRunner = new WfAppRunner();
            appRunner.ProcessGUID = process_guid;
            appRunner.AppInstanceID = application_instance_id;
            appRunner.AppName = application_name;
            appRunner.UserID = per_dict["limu"];
            appRunner.UserName = "limu";

            var wfService = new WorkflowService();
            var nextSteps = wfService.GetNextActivityTree(appRunner);

            if (nextSteps != null)
            {
                //构造下一步办理人的信息
                appRunner.NextActivityPerformers = CreateNextActivityPerformers(nextSteps);

                var r2 = wfService.RunProcessApp(appRunner);
                var msg2 = string.Format("执行【生产】:{0}, {1}\r\n", r2.Status, r2.Message);

                WriteText(msg2);

                if (r2.Status == WfExecutedStatus.Success)
                {
                    WriteText(string.Format("任务已经发送到下一节点:{0}\r\n\r\n", nextSteps[0].ActivityName));
                }
            }
            else
            {
                WriteText("您当前没有办理任务!\r\n");
            }
        }

        /// <summary>
        /// 质检节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQC_Click(object sender, EventArgs e)
        {
            WfAppRunner appRunner = new WfAppRunner();
            appRunner.ProcessGUID = process_guid;
            appRunner.AppInstanceID = application_instance_id;
            appRunner.AppName = application_name;
            appRunner.UserID = per_dict["limu"];
            appRunner.UserName = "limu";

            var wfService = new WorkflowService();
            var nextSteps = wfService.GetNextActivityTree(appRunner);

            if (nextSteps != null)
            {
                //构造下一步办理人的信息
                appRunner.NextActivityPerformers = CreateNextActivityPerformers(nextSteps);

                var r2 = wfService.RunProcessApp(appRunner);
                var msg2 = string.Format("执行【质检】:{0}, {1}\r\n", r2.Status, r2.Message);

                WriteText(msg2);

                if (r2.Status == WfExecutedStatus.Success)
                {
                    WriteText(string.Format("任务已经发送到下一节点:{0}\r\n\r\n", nextSteps[0].ActivityName));
                }
            }
            else
            {
                WriteText("您当前没有办理任务!\r\n");
            }
        }

        /// <summary>
        /// 称重节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWeight_Click(object sender, EventArgs e)
        {
            WfAppRunner appRunner = new WfAppRunner();
            appRunner.ProcessGUID = process_guid;
            appRunner.AppInstanceID = application_instance_id;
            appRunner.AppName = application_name;
            appRunner.UserID = per_dict["guangling"];
            appRunner.UserName = "guangling";

            var wfService = new WorkflowService();
            var nextSteps = wfService.GetNextActivityTree(appRunner);

            if (nextSteps != null)
            {
                //构造下一步办理人的信息
                appRunner.NextActivityPerformers = CreateNextActivityPerformers(nextSteps);

                var r2 = wfService.RunProcessApp(appRunner);
                var msg2 = string.Format("执行【称重】:{0}, {1}\r\n", r2.Status, r2.Message);

                WriteText(msg2);

                if (r2.Status == WfExecutedStatus.Success)
                {
                    WriteText(string.Format("任务已经发送到下一节点:{0}\r\n\r\n", nextSteps[0].ActivityName));
                }
            }
            else
            {
                WriteText("您当前没有办理任务!\r\n");
            }
        }

        /// <summary>
        /// 打印快递单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintLogistics_Click(object sender, EventArgs e)
        {
            WfAppRunner appRunner = new WfAppRunner();
            appRunner.ProcessGUID = process_guid;
            appRunner.AppInstanceID = application_instance_id;
            appRunner.AppName = application_name;
            appRunner.UserID = per_dict["guangling"];
            appRunner.UserName = "guangling";

            var wfService = new WorkflowService();
            var nextSteps = wfService.GetNextActivityTree(appRunner);

            if (nextSteps != null)
            {
                //构造下一步办理人的信息
                appRunner.NextActivityPerformers = CreateNextActivityPerformers(nextSteps);

                var r2 = wfService.RunProcessApp(appRunner);
                var msg2 = string.Format("执行【打印快递单】:{0}, {1}\r\n", r2.Status, r2.Message);
                WriteText(msg2);

                if (r2.Status == WfExecutedStatus.Success)
                {
                    WriteText(string.Format("任务已经发送到下一节点:{0}\r\n\r\n", nextSteps[0].ActivityName));
                }
            }
            else
            {
                WriteText("您当前没有办理任务!\r\n");
            }
        }

        /// <summary>
        /// 创建下一步的执行人员列表
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        private IDictionary<String, PerformerList> CreateNextActivityPerformers(IList<NodeView> nextStepList)
        {
            var nextDict = new Dictionary<String, PerformerList>();
            UserModel um = new UserModel();

            foreach (NodeView view in nextStepList)
            {
                PerformerList pl = new PerformerList();
                foreach (Role role in view.Roles)
                {
                    var a = um.GetUsersByRoleCode(role.RoleCode);      //根据角色代码获取人员
                    foreach (UserEntity u in a)
                    {
                        pl.Add(new Performer(u.ID.ToString(), u.UserName));
                    }
                }
                nextDict[view.ActivityGUID] = pl;
            }

            return nextDict;
        }

        /// <summary>
        /// 创建下一步的执行人员列表
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        private IDictionary<String, PerformerList> CreateNextActivityPerformers(NodeView view)
        {
            var nextDict = new Dictionary<String, PerformerList>();
            UserModel um = new UserModel();

            PerformerList pl = new PerformerList();
            foreach (Role role in view.Roles)
            {
                var a = um.GetUsersByRoleCode(role.RoleCode);      //根据角色代码获取人员
                foreach (UserEntity u in a)
                {
                    pl.Add(new Performer(u.ID.ToString(), u.UserName));
                }
            }
            nextDict[view.ActivityGUID] = pl;

            return nextDict;
        }

        /// <summary>
        /// 显示执行日志
        /// </summary>
        /// <param name="text"></param>
        private void WriteText(string text)
        {
            if (this.InvokeRequired)
            {
                RefreshTextDelegate del = new RefreshTextDelegate(WriteText);
                this.Invoke(del, new object[] { text });
            }
            else
            {
                this.textBox1.Text += text;
            }
        }
    }
}
