using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Common;
using Slickflow.Engine.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Slickflow.Winform
{
    public partial class FormOfficeIn : Form
    {
        private string application_instance_id = "2";
        private string process_guid = "68696ea3-00ab-4b40-8fcf-9859dbbde378";

        public FormOfficeIn()
        {
            InitializeComponent();
        }

        private void btn开始_Click(object sender, EventArgs e)
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
            var msg = string.Format("流程运行结果：{0}\r\n{1}\r\n", result.Status, result.Message);
            tBoxResult.Text += msg;
        }

        private void btn仓库签字_Click(object sender, EventArgs e)
        {
            WfAppRunner appRunner = new WfAppRunner();
            appRunner.ProcessGUID = process_guid;
            appRunner.AppInstanceID = application_instance_id;
            //第一步任务承担者，登录后要找到自己的任务
            appRunner.AppName = "officeIn";
            appRunner.UserID = "1";
            appRunner.UserName = "user1";
            IWorkflowService wfService = new WorkflowService();

            //外部变量条件
            Dictionary<string, string> dictCondition = new Dictionary<string, string>();
            dictCondition.Add("surplus", cBoxSurplus.Text);
            appRunner.Conditions = dictCondition;

            //动态获取下一跳转后的节点
            NodeView nv = wfService.GetNextActivity(appRunner, dictCondition);
            //指定对象执行者
            PerformerList list = new PerformerList();
            list.Add(new Performer("3", "user3"));
            Dictionary<String, PerformerList> dictPerformer = new Dictionary<String, PerformerList>();
            dictPerformer.Add(nv.ActivityGUID, list);
            appRunner.NextActivityPerformers = dictPerformer;

            var result = wfService.RunProcessApp(appRunner);
            var msg = string.Format("流程运行结果：{0}\r\n{1}\r\n", result.Status, result.Message);
            tBoxResult.Text += msg;
        }

        private void btn综合部签字_Click(object sender, EventArgs e)
        {
            WfAppRunner appRunner = new WfAppRunner();
            appRunner.ProcessGUID = process_guid;
            appRunner.AppInstanceID = application_instance_id;
            //第一步任务承担者，登录后要找到自己的任务
            appRunner.AppName = "officeIn";
            appRunner.UserID = "3";
            appRunner.UserName = "user3";
            IWorkflowService wfService = new WorkflowService();

            PerformerList list = new PerformerList();
            list.Add(new Performer("4", "user4"));
            NodeView nv = wfService.GetNextActivity(appRunner);
            Dictionary<String, PerformerList> dictPerformer = new Dictionary<String, PerformerList>();
            dictPerformer.Add(nv.ActivityGUID, list);
            appRunner.NextActivityPerformers = dictPerformer;
            var result = wfService.RunProcessApp(appRunner);
            var msg = string.Format("流程运行结果：{0}\r\n{1}\r\n", result.Status, result.Message);
            tBoxResult.Text += msg;
        }

        private void btn总经理签字_Click(object sender, EventArgs e)
        {
            WfAppRunner appRunner = new WfAppRunner();
            appRunner.ProcessGUID = process_guid;
            appRunner.AppInstanceID = application_instance_id;
            //第一步任务承担者，登录后要找到自己的任务
            appRunner.AppName = "officeIn";
            appRunner.UserID = "4";
            appRunner.UserName = "user4";
            IWorkflowService wfService = new WorkflowService();
            PerformerList list = new PerformerList();
            list.Add(new Performer("4", "user4"));
            NodeView nv = wfService.GetNextActivity(appRunner);
            Dictionary<String, PerformerList> dictPerformer = new Dictionary<String, PerformerList>();
            dictPerformer.Add(nv.ActivityGUID, list);
            appRunner.NextActivityPerformers = dictPerformer;
            var result = wfService.RunProcessApp(appRunner);
            var msg = string.Format("流程运行结果：{0}\r\n{1}\r\n", result.Status, result.Message);
            tBoxResult.Text += msg;
        }
    }
}
