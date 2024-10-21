using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Service;
using Slickflow.Engine.Xpdl;

namespace Slickflow.WebApi.Controllers
{

    //webapi: http://localhost/sfapi/api/wfevent/
    //数据库表: WfProcess
    //流程记录ID：219
    //流程名称：事件测试交互流程
    //GUID: 4be58a96-926c-4aff-a383-fe71185572e5
    //startup process:
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"4be58a96-926c-4aff-a383-fe71185572e5"}

    //run process app:
    ////订单处理节点：
    ////下一步是结束节点
    //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"4be58a96-926c-4aff-a383-fe71185572e5","UserID":"10","UserName":"Long","NextActivityPerformers":{"de50335a-034c-4c58-db72-ddd00c1aebfe":[{"UserID":10,"UserName":"Long"}]}}

    /// <summary>
    /// 事件控制器
    /// </summary>
    public class WfBasicController : Controller
    {
        #region Workflow 数据访问基本操作
        [HttpGet]
        public string Hello()
        {
            return "Hello World!";
        }

        // GET: /Workflow/
        [HttpGet]
        public object Get()
        {
            ProcessManager pm = new ProcessManager();
            var process = pm.GetByVersion("5d6a7d6f-daa2-482d-8303-87b3b9f59a6a", "1");      //版本默认为1

            return process;
        }

        [HttpPost]
        public void InsertProcess([FromBody] ProcessEntity obj)
        {
            ProcessManager pm = new ProcessManager();
            pm.Insert(obj);
        }

        [HttpPost]
        public void UpdateProcess([FromBody] ProcessEntity obj)
        {
            ProcessManager pm = new ProcessManager();
            pm.Update(obj);
        }

        [HttpPost]
        public void RemoveProcess([FromBody] ProcessEntity entity)
        {
            ProcessManager pm = new ProcessManager();
            pm.Delete(entity.ProcessGUID, entity.Version);
        }

        [HttpGet]
        public ProcessInstanceEntity GetProcessInstance()
        {
            IWorkflowService service = new WorkflowService();
            var instance = service.GetProcessInstance(14001);

            return instance;
        }

        [HttpGet]
        public ActivityInstanceEntity GetActivityInstance()
        {
            IWorkflowService service = new WorkflowService();
            var instance = service.GetActivityInstance(100);

            return instance;
        }

        [HttpGet]
        public void TestProcessModel()
        {
            var processGUID = "f2f698de-9204-4331-bd41-d6dae15c06a3";
            var version = "1";
            var processModel = ProcessModelFactory.CreateByProcess(processGUID, version);
            var activityList = processModel.GetActivityList();
        }
        #endregion
    }
}

