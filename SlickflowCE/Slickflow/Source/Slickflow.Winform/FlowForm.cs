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
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Service;

namespace Slickflow.Winform
{
    /// <summary>
    /// 流程流转测试页面
    /// </summary>
    public partial class FlowForm : Form
    {
        private string application_instance_id = "1";
        //流程ID
        private string process_guid = "68696ea3-00ab-4b40-8fcf-9859dbbde378";

        public FlowForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartup_Click(object sender, EventArgs e)
        {
            WfAppRunner appRunner = new WfAppRunner();
            appRunner.ProcessGUID = process_guid;
            appRunner.AppInstanceID = application_instance_id;
            //指定第一步的处理人
            appRunner.AppName = "officeIn";
            appRunner.UserID = "1";
            appRunner.UserName = "user1";
            IWorkflowService wfService = new WorkflowService();
            var result = wfService.StartProcess(appRunner);
            var msg = string.Format("流程启动结果：{0}\r\n", result.Status);
            textBox1.Text += msg;
        }

        /// <summary>
        /// 运行流程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRun_Click(object sender, EventArgs e)
        {
            WfAppRunner appRunner = new WfAppRunner();
            appRunner.ProcessGUID = process_guid;
            appRunner.AppInstanceID = application_instance_id;
            //第一步任务承担者，登录后要找到自己的任务
            appRunner.AppName = "officeIn";
            appRunner.UserID = "1";
            appRunner.UserName = "user1";
            IWorkflowService wfService = new WorkflowService();
            var en = new TaskQuery
            {
                UserID = "1"
            };
            IList<TaskViewEntity> taskViewList = wfService.GetReadyTasks(en);
            if (taskViewList != null)
            {
                dataGridView1.DataSource = taskViewList;
            }
            ////下一步执行人
            //PerformerList list = new PerformerList();
            //Performer p = new Performer("3", "user3");
            //下一步人ID，Name 注意有角色区分
            //list.Add(p);
            //Dictionary<string, string> dictCondition = new Dictionary<string, string>();
            //dictCondition.Add("IsHavingWeight", "true");
            //dictCondition.Add("CanUseStock", "false");
            //appRunner.Conditions = dictCondition;
            //NodeView nv = wfService.GetNextActivity(appRunner);//, dictCondition
            //Dictionary<String, PerformerList> dictPerformer = new Dictionary<String, PerformerList>();
            //dictPerformer.Add(nv.ActivityGUID, list);
            //appRunner.NextActivityPerformers = dictPerformer;

            var result = wfService.RunProcessApp(appRunner);
            var msg = string.Format("流程运行结果：{0}\r\n", result.Status);
            textBox1.Text += msg;
        }

        /// <summary>
        /// XML节点预定义跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSkip_Click(object sender, EventArgs e)
        {
            WfAppRunner appRunner = new WfAppRunner();
            appRunner.ProcessGUID = process_guid;
            appRunner.AppInstanceID = application_instance_id;
            appRunner.AppName = "WallwaOrder";
            appRunner.UserID = "13";
            appRunner.UserName = "andun";

            IWorkflowService wfService = new WorkflowService();
            var nodeViewList = wfService.GetNextActivityTree(appRunner);
            if (nodeViewList != null && nodeViewList.Count() == 1)
            {
                var nodeView = nodeViewList[0];
                if (nodeView.IsSkipTo == true)
                {
                    //下一步执行人
                    PerformerList list = new PerformerList();
                    Performer p = new Performer("1", "admin");//下一步人ID，Name
                    list.Add(p);
                    Dictionary<String, PerformerList> dict = new Dictionary<String, PerformerList>();
                    dict.Add(nodeView.ActivityGUID, list); //nodeView.ID  下一步节点的标识ID
                    appRunner.NextActivityPerformers = dict;

                    var result = wfService.JumpProcess(appRunner);
                    var msg = string.Format("流程跳转结果：{0}\r\n", result.Status);
                    textBox1.Text += msg;
                }
            }
        }

        /// <summary>
        /// 跳转流程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnJump_Click(object sender, EventArgs e)
        {
            WfAppRunner appRunner = new WfAppRunner();
            appRunner.ProcessGUID = process_guid;
            appRunner.AppInstanceID = application_instance_id;
            appRunner.AppName = "WallwaOrder";
            appRunner.UserID = "1";
            appRunner.UserName = "admin";

            //下一步执行人
            PerformerList list = new PerformerList();
            Performer p = new Performer("13", "andun");//下一步人ID，Name
            list.Add(p);
            Dictionary<String, PerformerList> dict = new Dictionary<String, PerformerList>();
            dict.Add("7c1aa9f9-7f0f-46bf-a219-0b80fdfbbe3d", list); //print activity:"fc8c71c5-8786-450e-af27-9f6a9de8560f"下一步节点的标识ID
            appRunner.NextActivityPerformers = dict;

            IWorkflowService wfService = new WorkflowService();
            var result = wfService.JumpProcess(appRunner);

            var msg = string.Format("流程跳转结果：{0}\r\n", result.Status);
            textBox1.Text += msg;
        }

        /// <summary>
        /// 退回流程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBackward_Click(object sender, EventArgs e)
        {
            WfAppRunner appRunner = new WfAppRunner();
            appRunner.ProcessGUID = process_guid;
            appRunner.AppInstanceID = application_instance_id;
            appRunner.AppName = "WallwaOrder";
            appRunner.UserID = "1";
            appRunner.UserName = "admin";

            //下一步执行人
            PerformerList list = new PerformerList();
            Performer p = new Performer("13", "andun");//下一步人ID，Name
            list.Add(p);
            Dictionary<String, PerformerList> dict = new Dictionary<String, PerformerList>();
            dict.Add("fc8c71c5-8786-450e-af27-9f6a9de8560f", list); //print activity:"fc8c71c5-8786-450e-af27-9f6a9de8560f"下一步节点的标识ID
            appRunner.NextActivityPerformers = dict;

            IWorkflowService wfService = new WorkflowService();
            var result = wfService.JumpProcess(appRunner);

            var msg = string.Format("流程跳转回退结果：{0}\r\n", result.Status);
            textBox1.Text += msg;
        }

        /// <summary>
        /// 撤销流程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWithdraw_Click(object sender, EventArgs e)
        {
            WfAppRunner appRunner = new WfAppRunner();
            appRunner.ProcessGUID = process_guid;
            appRunner.AppInstanceID = application_instance_id;
            appRunner.AppName = "WallwaOrder";
            appRunner.UserID = "1";
            appRunner.UserName = "admin";

            IWorkflowService wfService = new WorkflowService();
            var result = wfService.WithdrawProcess(appRunner);
            var msg = string.Format("流程撤销结果：{0}\r\n", result.Status);
            textBox1.Text += msg;
        }

        /// <summary>
        /// 退回流程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendback_Click(object sender, EventArgs e)
        {
            WfAppRunner appRunner = new WfAppRunner();
            appRunner.ProcessGUID = process_guid;
            appRunner.AppInstanceID = application_instance_id;
            appRunner.AppName = "WallwaOrder";
            appRunner.UserID = "13";
            appRunner.UserName = "andun";

            IWorkflowService wfService = new WorkflowService();
            var result = wfService.SendBackProcess(appRunner);
            var msg = string.Format("流程退回结果：{0}\r\n", result.Status);
            textBox1.Text += msg;
        }

        /// <summary>
        /// 返签流程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReverse_Click(object sender, EventArgs e)
        {

        }
    }
}
