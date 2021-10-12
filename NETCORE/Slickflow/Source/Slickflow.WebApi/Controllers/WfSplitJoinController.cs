/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。
*  
The Slickflow project.
Copyright (C) 2014  .NET Workflow Engine Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SlickOne.WebUtility;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Service;

namespace Slickflow.WebApi.Controllers
{
    //webapi: http://localhost/sfapi/api/wfsplitjoin/
    //分支流程基本测试
    //流程调用JSON格式说明：
    //流程id: 73
    //流程GUID: 3a8ce214-fd18-4fac-95c0-e7958bc1b2f8
    //1. officeIn.orSplit-end.xml
    //startup process:
    //{"UserID":"6","UserName":"XiaoMing","AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"3a8ce214-fd18-4fac-95c0-e7958bc1b2f8"}

    //run process app:
    ////分支1--行政部签字(xml 文件: officeIn.orSplit-end.xml)
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"3a8ce214-fd18-4fac-95c0-e7958bc1b2f8","UserID":"6","UserName":"XiaoMing","Conditions":{"surplus":"normal"}, "NextActivityPerformers":{"c3cbb3cc-fa60-42ad-9a10-4ec2638aff49":[{"UserID":4,"UserName":"MissLi"}]}}

    //run process app:
    //同时分发到两个分支执行
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"3a8ce214-fd18-4fac-95c0-e7958bc1b2f8","UserID":"6","UserName":"XiaoMing","Conditions":{"surplus":"normal"}, "NextActivityPerformers":{"c3cbb3cc-fa60-42ad-9a10-4ec2638aff49":[{"UserID":4,"UserName":"MissLi"}], "9414c43c-0c8c-4c0b-b65d-16203288c7ca":[{"UserID":24,"UserName":"MrsQiao"}]}}

    //run process app:
    //行政部分支执行
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"3a8ce214-fd18-4fac-95c0-e7958bc1b2f8","UserID":"4","UserName":"MissLi", "NextActivityPerformers":{"932f7fa0-2d4c-4257-c158-b8b181af2d0a":[{"UserID":10,"UserName":"Long"}]}}

    //run process app:
    //财务部分支执行
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"3a8ce214-fd18-4fac-95c0-e7958bc1b2f8","UserID":"24","UserName":"MrsQiao", "NextActivityPerformers":{"932f7fa0-2d4c-4257-c158-b8b181af2d0a":[{"UserID":10,"UserName":"Long"}]}}


    //end node
    //结束节点
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"3a8ce214-fd18-4fac-95c0-e7958bc1b2f8","UserID":"10","UserName":"Long", "NextActivityPerformers":{"76f7ef75-b538-40c8-b529-0849ca777b94":[{"UserID":10,"UserName":"Long"}]}}

    //withdraw process:
    //撤销回上一步节点，由仓库签字人员，从行政部签字节点撤销回来
    //{"UserID":"6","UserName":"XiaoMing","AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"3a8ce214-fd18-4fac-95c0-e7958bc1b2f8"}

    //2. officeIn.orSplitorJoin.xml
    //startup process:
    //{"UserID":"10","UserName":"Long","AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"3a8ce214-fd18-4fac-95c0-e7958bc1b2f8"}

    ////分支(xml 文件: officeIn.orSplitorJoin.xml)
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"3a8ce214-fd18-4fac-95c0-e7958bc1b2f8","UserID":"10","UserName":"Long","Conditions":{"surplus":"超量"}, "NextActivityPerformers":{"e60084e4-517a-4892-a290-517159f1b7f4":[{"UserID":10,"UserName":"Long"}]}}

    //cross andjoin
    //合并节点后的节点（总经理签字）执行
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"3a8ce214-fd18-4fac-95c0-e7958bc1b2f8","UserID":"10","UserName":"Long", "NextActivityPerformers":{"0fdff3c0-be97-43d6-b4ff-90d52efb5d6f":[{"UserID":10,"UserName":"Long"}]}}

    //end node
    //结束节点
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"3a8ce214-fd18-4fac-95c0-e7958bc1b2f8","UserID":"10","UserName":"Long", "NextActivityPerformers":{"76f7ef75-b538-40c8-b529-0849ca777b94":[{"UserID":10,"UserName":"Long"}]}}


    //3. officeIn.andSplitandJoin.xml
    //startup process:
    //{"UserID":"10","UserName":"Long","AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"68696ea3-00ab-4b40-8fcf-9859dbbde378"}

    ////AndSplit/AndJoin
    //两个分支同时执行(xml 文件: officeIn.andSplitandJoin.xml)
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"68696ea3-00ab-4b40-8fcf-9859dbbde378","UserID":"10","UserName":"Long","Conditions":{"surplus":"高量"}, "NextActivityPerformers":{"e60084e4-517a-4892-a290-517159f1b7f4":[{"UserID":10,"UserName":"Long"}], "ce3343b6-930d-4962-a2b9-2c4c4b2dab06":[{"UserID":20,"UserName":"Minlingda"}]}}

    //AndSplit 之后，并行任务的执行
    //带任务ID的Json数据，用于AndSplit产生的个人用户同时存在多个任务的案例
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"68696ea3-00ab-4b40-8fcf-9859dbbde378","TaskID":"18088","UserID":"10", "UserName":"Long", "NextActivityPerformers":{"0fdff3c0-be97-43d6-b4ff-90d52efb5d6f":[{"UserID":10,"UserName":"Long"}]}}
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"68696ea3-00ab-4b40-8fcf-9859dbbde378", "TaskID":"18087","UserID":"20", "UserName":"Minlingda", "NextActivityPerformers":{"0fdff3c0-be97-43d6-b4ff-90d52efb5d6f":[{"UserID":10,"UserName":"Long"}]}}

    //cross andjoin
    //合并节点后的节点（总经理签字）执行
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"68696ea3-00ab-4b40-8fcf-9859dbbde378","UserID":"10","UserName":"Long", "NextActivityPerformers":{"0fdff3c0-be97-43d6-b4ff-90d52efb5d6f":[{"UserID":10,"UserName":"Long"}]}}

    //end node
    //结束节点
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"68696ea3-00ab-4b40-8fcf-9859dbbde378","UserID":"10","UserName":"Long", "NextActivityPerformers":{"76f7ef75-b538-40c8-b529-0849ca777b94":[{"UserID":10,"UserName":"Long"}]}}


    //4.officeIn.xorSplitxorJoin.xml
    ////startup process:
    //{"UserID":"10","UserName":"Long","AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"68696ea3-00ab-4b40-8fcf-9859dbbde378"}

    //run process:
    //transition priority优先级高的传入，先满足条件表达式的，先执行
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"68696ea3-00ab-4b40-8fcf-9859dbbde378","UserID":"10","UserName":"Long","Conditions":{"surplus":"海量"}, "NextActivityPerformers":{"e60084e4-517a-4892-a290-517159f1b7f4":[{"UserID":10,"UserName":"Long"}]}}

    //cross xorjoin
    //合并节点后的节点（总经理签字）执行
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"68696ea3-00ab-4b40-8fcf-9859dbbde378","UserID":"10","UserName":"Long", "NextActivityPerformers":{"0fdff3c0-be97-43d6-b4ff-90d52efb5d6f":[{"UserID":10,"UserName":"Long"}]}}

    //end node
    //结束节点
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"68696ea3-00ab-4b40-8fcf-9859dbbde378","UserID":"10","UserName":"Long", "NextActivityPerformers":{"76f7ef75-b538-40c8-b529-0849ca777b94":[{"UserID":10,"UserName":"Long"}]}}


    //5. run sub process
    //startup process:
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d"}

    //有子流程
    //启动子流程
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"68696ea3-00ab-4b40-8fcf-9859dbbde378","UserID":"10","UserName":"Long","NextActivityPerformers":{"5fa796f6-2d5d-4ed6-84e2-a7c4e4e6aabc":[{"UserID":10,"UserName":"Long"}]}}

    //withdraw process:
    //撤销至上一步节点（由财务审批到上一步前台办理）
    //{"UserID":"6","UserName":"XiaoMing","AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"68696ea3-00ab-4b40-8fcf-9859dbbde378"}

    //runprocess app
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"68696ea3-00ab-4b40-8fcf-9859dbbde378","UserID":"10","UserName":"Long","NextActivityPerformers":{"39c71004-d822-4c15-9ff2-94ca1068d745":[{"UserID":10,"UserName":"Long"}]}}

    //财务审批办理节点：
    ////下一步是结束节点
    //{"UserID":"10","UserName":"Long","AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"68696ea3-00ab-4b40-8fcf-9859dbbde378","NextActivityPerformers":{"b70e717a-08da-419f-b2eb-7a3d71f054de":[{"UserID":10,"UserName":"Long"}]}}

    //reverse process:
    //返签
    //{"UserID":"10","UserName":"Long","AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"68696ea3-00ab-4b40-8fcf-9859dbbde378"}

    //sendback process
    //退回
    //数据格式同返签(撤销,退回,返签Json数据格式相同.)

    //read task, and make activity running:
    //任务阅读：
    //{"UserID":"10","UserName":"Long","TaskID":"17"}}

    //获取下一步办理步骤：
    //1) 根据应用来获取
    //GetNextSteps
    //{"AppName":"OfficeIn","AppInstanceID":915,"UserID":"10","UserName":"Long","ProcessGUID":"68696ea3-00ab-4b40-8fcf-9859dbbde378","NextActivityPerformers":{"39c71004-d822-4c15-9ff2-94ca1068d745":[{"UserID":"10","UserName":"Long"}]},"Flowstatus":"启动"}

    //2) 根据任务ID来获取
    //GetTaskNextSteps

    //撤销流程: WithdrawProcess
    //退回流程：SendBackProcess
    //返签流程：ReverseProcess
    //取消运行流程：CancelProcess
    //废弃所有流程实例：DiscardProcess
    /// <summary>
    /// </summary>
    public class WfSplitJoinController : Controller
    {
        //
        // GET: /WorkflowPL/

        #region Workflow Api访问操作
        [HttpPost]
        public ResponseResult StartProcess([FromBody] WfAppRunner starter)
        {
            IWorkflowService wfService = new WorkflowService();
            IDbConnection conn = SessionFactory.CreateConnection();

            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                WfExecutedResult result = wfService.StartProcess(conn, starter, trans);
                trans.Commit();

                int newProcessInstanceID = result.ProcessInstanceIDStarted;
                if (result.Status == WfExecutedStatus.Success)
                {
                    return ResponseResult.Success();
                }
                else
                {
                    return ResponseResult.Error(result.Message);
                }
            }
            catch (WorkflowException w)
            {
                trans.Rollback();
                return ResponseResult.Error(w.Message);
            }
            finally
            {
                trans.Dispose();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        [HttpPost]
        public ResponseResult RunProcess([FromBody] WfAppRunner runner)
        {
            IWorkflowService wfService = new WorkflowService();
            IDbConnection conn = SessionFactory.CreateConnection();

            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = wfService.RunProcess(conn, runner, trans);
                
                if (result.Status == WfExecutedStatus.Success){
                    trans.Commit();
                    return ResponseResult.Success();
                }
                else
                {
                    trans.Rollback();
                    return ResponseResult.Error(result.Message);
                }
            }
            catch (WorkflowException w)
            {
                trans.Rollback();
                return ResponseResult.Error(w.Message);
            }
            finally
            {
                trans.Dispose();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        [HttpPost]
        public ResponseResult WithdrawProcess([FromBody] WfAppRunner runner)
        {
            IWorkflowService wfService = new WorkflowService();
            IDbConnection conn = SessionFactory.CreateConnection();

            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = wfService.WithdrawProcess(conn, runner, trans);

                if (result.Status == WfExecutedStatus.Success)
                {
                    trans.Commit();
                    return ResponseResult.Success();
                }
                else
                {
                    trans.Rollback();
                    return ResponseResult.Error(result.Message);
                }
            }
            catch (WorkflowException w)
            {
                trans.Rollback();
                return ResponseResult.Error(w.Message);
            }
            finally
            {
                trans.Dispose();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        [HttpPost]
        public ResponseResult SendBackProcess([FromBody] WfAppRunner runner)
        {
            IWorkflowService wfService = new WorkflowService();
            IDbConnection conn = SessionFactory.CreateConnection();

            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = wfService.SendBackProcess(conn, runner, trans);
                trans.Commit();

                if (result.Status == WfExecutedStatus.Success)
                    return ResponseResult.Success();
                else
                    return ResponseResult.Error(result.Message);
            }
            catch (WorkflowException w)
            {
                trans.Rollback();
                return ResponseResult.Error(w.Message);
            }
            finally
            {
                trans.Dispose();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        [HttpPost]
        public ResponseResult ReverseProcess([FromBody] WfAppRunner runner)
        {
            IWorkflowService wfService = new WorkflowService();
            IDbConnection conn = SessionFactory.CreateConnection();

            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = wfService.ReverseProcess(conn, runner, trans);
                trans.Commit();

                if (result.Status == WfExecutedStatus.Success)
                    return ResponseResult.Success();
                else
                    return ResponseResult.Error(result.Message);
            }
            catch (WorkflowException w)
            {
                trans.Rollback();
                return ResponseResult.Error(w.Message);
            }
            finally
            {
                trans.Dispose();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        [HttpPost]
        public ResponseResult DiscardProcess([FromBody] WfAppRunner discarder)
        {
            IWorkflowService service = new WorkflowService();
            var result = service.DiscardProcess(discarder);

            return ResponseResult.Success();
        }

        [HttpPost]
        public ResponseResult GetNextActivityRoleUserTree([FromBody] WfAppRunner nexter)
        {
            IWorkflowService service = new WorkflowService();
            var nodeViewList = service.GetNextActivityRoleUserTree(nexter, nexter.Conditions);

            return ResponseResult.Success(nodeViewList.Count().ToString());
        }
        #endregion

    }
}
