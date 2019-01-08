## Slickflow

.NET Workflow Engine With Full Source Code 
1. **.NET, .NET CORE version both supported**  
 Slickflow is an open source project based on .NET Framework 4.5, .NET CORE 2; It's easy to use engine product into cross platform applilcation.  
2. **BPMN graphic style process diagram**   
 Slickflow is using BPMN natation to descript process diagram, the Slickflow designer is HTML5 graph editor and user friendly to business process communicaiton and business analysis.  
3. **High performance with Dapper.NET library**  
 Dapper is a simple object mapper for .NET and own the title of King of Micro ORM in terms of speed and is virtually as fast as using a raw ADO.NET data reader. An ORM is an Object Relational Mapper, which is responsible for mapping between database and programming language.
(Ref: https://dapper-tutorial.net/dapper)  
 4. **Multiple database supported**  
 Slickflow supports SQLSERVER, ORACLE, MySQL and other database, it implemented by Dapper.NET extension library. The .net core version using EF core to support different database products.  
5. **Workflow patterns supported**  
    ![Wokflow Pattern](http://www.slickflow.com/content/img/sfterm.png)  

**1) sequence**  
    the most frequently process pattern   
**2) split/merge**  
   support and/or gateway such as **and/or split**, **and/or join**, together with  condition variables on the transition  
**3) sub-process**  
    in the main process, a sub process node can start a new process life cycle.  
**4) multi-instance**  
   muptile performers processing a task together by multiple task instances. All performer both compete their task, then the process can be continued on. There are **sequence** and **parallel** pattern, and the **percentage** or **count** parameters can be set on it to ensue when can go to next step.  
    ![Muliple Instance Pattern](http://www.slickflow.com/content/img/wfpattern-mi.png)  

**5) event interoperation**  
   process instance and activity instance event delegation service, such as process/activity start, execute and complete.  
**6) timer**  
   integrated with **HangFire** library, and with **CRON** expression supported  
**7) email**  
   todo or overdue taks email notification  
**8) withdraw**  
	withdraw the task after just sent out to next step users.  
**9) sendback**  
    send back to previous step user, becuase some exceptions.  
**10) resend**  
    combined after sendback and re-send the task to origianl sendback users.  
**11) reverse**  
    reverse the process instance alive when completed them.  
**12) jump**  
    jump the process over by serveral steps forward or backward.  
 
**6. Process Version**  
    the process has version property to upgrade a new definiton.  
**7. XML Cache**    
    the runtime instance use cache to keep xml process diagram in the memory.  
**8. Code Style**    
**0) model**  
    
    //create a simple sequence process diagram by hand code rather than a HTML designer
    var processGUID = Guid.NewGuid().ToString();
    IProcessModelBuilder pmb = new ProcessModelBuilder();
    var entity = pmb.Create(processGUID, "1")
        .CreateActivity(ActivityTypeEnum.StartNode, "Start", "Start")
        .CreateActivity(ActivityTypeEnum.TaskNode, "Task1", "Task1")
        .CreateActivity(ActivityTypeEnum.TaskNode, "Task2", "Task2")
        .CreateActivity(ActivityTypeEnum.EndNode, "End", "End")
        .Sequence();
	
	
   ![simple sequence diagram](http://www.slickflow.com/content/img/simple-sequence.png)  
    
                
**1) start**  
    
    //start a new process instance
    IWorkflowService wfService = new WorkflowService();
    var wfResult = wfService.CreateRunner(runner.UserID, runner.UserName)
             .UseApp(runner.AppInstanceID, runner.AppName, runner.AppInstanceCode)
             .UseProcess(runner.ProcessGUID, runner.Version)
             .Subscribe(EventFireTypeEnum.OnProcessStarted, (id, processGUID, delegateService) => {
                 var processInstanceID = id;
                 delegateService.SetVariable("name", "book");
                 delegateService.SetVariable("amount", "30");
                 return true;
             })
             .Start();

   **2) run**  
    
    //run a process instance to next step
    IWorkflowService wfService = new WorkflowService();
    var wfResult = wfService.CreateRunner(runner.UserID, runner.UserName)
             .UseApp(runner.AppInstanceID, runner.AppName, runner.AppInstanceCode)
             .UseProcess(runner.ProcessGUID, runner.Version)
             .NextStep(runner.NextActivityPerformers)
             .IfCondition(runner.Conditions)	//condition on the transiton
             .Subscribe(EventFireTypeEnum.OnActivityExecuting, (activityInstanceID, activityCode, delegateService) => {
                 if (activityCode == "Task1")
                 {
                     delegateService.SetVariable("name", "book-task1");
                     delegateService.SetVariable("amount", "50");
                 }
                 return true;
             })
             .Run();

    
**9. Rich demo projects**  
 WebDemo, MvcDemo and WinformDemo project are domonstated for different type enterprise information system.   
**10. Target**  
 Slickflow is very suitable for software teams or companies who want to  integrat workflow engine into their products.  
**11. Suggestions**  
 Slickflow is suggusted to give programmers an flexible way to integrate workflow engine component into their products or customer projects. The programmers can write their own code segemnts based on the engine component.   
**12. License**    
 1) Community version  
 The community version is under LGPL license.    
 2) **commercial license**  
 The commercial version provide technical support and version upgrade for customers.   
 
 if you have any further inquery, please feel free to contact us:   

**EMail: sales@ruochisoft.com**  
**QQ(Author): 47743901**

**Quick Start:**  
https://github.com/besley/Slickflow/wiki  
**Slickflow website:**  
http://www.slickflow.com  
**Demo:**  
http://www.slickflow.com/demo/index  
**Designer Demo:**  
http://demo.slickflow.com/slickflowdesigner/  
**Document:**  
http://www.slickflow.com/wiki/index  

**Slickflow(1.7.0.0) 企业版：**  
2019-1-8  
1.增加中间事件(IntermediateEvent)节点，处理流程状态通知事件；  
2.增加流程变量和活动变量，用于事件交互过程中的变量读取和设置；  
3.完成返送(Resend)接口，用于流程退回后的原路返回。  

**Slickflow(1.6.3.0)集团版**  
2018-10-26  
1.集成表单项目（百度编辑器版本）  
2.集成引擎项目，表单流转统一界面按钮定制功能实现  
3.表单字段绑定流程节点的权限功能实现，权限有（可见和读写）  
4.数据库表扩展增加租户属性字段，用于集团或SAAS平台项目搭建；  

**Slickflow(1.6.3.0) 企业版：**
2018-10-10  
1. 新增返送节点ResendProcess(); 用于退回后的原路返送；  
2. SendBackProcess()接口中，增加TaskID属性，用于多并行节点退回处理；  
参考WebApi项目, WfResendController.cs的测试用例  

2018-06-22  
1. 数据库更新  
1). 活动实例表WfActivityInstance表中增加逾期时间和处理字段  
   OverdueDateTime      datetime             null,  
   OverdueTreatedDateTime datetime             null,  
2). 任务表WfTasks表中增加邮件是否发送的字段位  
   IsEMailSent          tinyint              not null default 0,  
3). 增加作业表WhJobSchedule和WhJobLog表，用于HangFire作业调度  

2. 待办任务的邮件发送功能完成；  
3. 任务逾期超时，自动完成功能完成。  

2018-06-10   
1. 修正图形设计器连线重叠功能，即增加连线控制点；   
2. 完成待办任务邮件发送功能，定时轮询异步邮件发送；  


2018-05-25  
1. 修正设计器项目角色冗余数据；  
2. 增加ResetCache()接口，用于引擎XML缓存文件更新；  


**Slickflow(1.6.0.0) .NET CORE社区版更新说明**  
2018-04-02  
1. 支持.NET CORE2.0,实现跨平台程序分发；    
2. 采用EFCore框架，支持多数据库(SQLServer/MySQL/Oracle等)；
3. 实现CodeFirst数据库构建，附数据库生成项目；
4. ASP.NET MVC CORE项目示例，完整订单流程实例；

**Slickflow(1.6.0.0) 企业版更新说明**  
2017-12-10  
1. 引擎实现并行分支多实例（并行容器）的工作流模式；  
2. 设计器增加组合（Group）框选功能；  
3. 设计器增加定时结束类型节点，引擎实现流程定时结束功能；  
4. 增加流程图显示已完成路径的颜色显示（默认为红色）；  
   具体方法参见：kmain.renderCompletedTransitions()；  
5. 改进子流程启动过程的人员自动获取；  
6. Slickflow 集团版、多组户和SAAS平台版实现；  
1) 演示地址：http://gc.slickflow.com/sfadmin/  
2) 用户名密码：admin/123456  
数据库更新：  
WfProcess表变化字段（用于定时开始和定时结束场景）：  
1). StartType--开始类型  
2). StartExpression--开始表达式  
3). EndType--结束类型  
4). EndExpression--结束表达式  

2017-09-26  
自动定时任务模块：  
1. 设计器增加任务定时CRON表达式编辑器；  
2. 集成HangFire 任务定时作业组件，实现如下两个功能：  
1). 实现流程逾期自动结束任务作业；  
2). 实现流程定时启动任务作业；  
3. 新增WfJobs表，用于记录自动定时作业日志；  
4. 引擎接口读取所有任务类型节点列表GetAllTaskActivityList()方法改进：  
   按照流程图转移顺序返回活动节点列表数据；
   DEMO项目流程图显示MxGraph更新：
5. 业务项目的流程图查看功能更新；  
   WebDEMO, MvcDEMO 流程图查看功能整合到SFD设计器项目，不再重复构建；  
6. WebDEMO请假流程增减意见表(HrsLeaveOpinion)；  

**Slickflow(1.5.9) 企业版更新说明**  
1. 基于MxGraph的新版设计器发布；  
2. 设计器实现泳道(Swimlanes)功能；  
3. 设计器增加Actions 外部事件列表方法；  
4. 集成HangFire任务定时作业组件，实现流程逾期结束；  

**Slickflow(1.5.8) 企业版更新说明:**  
1. 增加加签通过率类型字段CompareType，用于加签办理页面动态指定（非设计器指定，是运行时决策指定）变量传入；  

示例如下：

        //动态变量数据格式(包含在WfAppRunner属性中)
        "DynamicVariables": {
            "SignForwardType": "SignForwardBefore",
            "SignForwardCompleteOrder": 2,
            "CompareType":  "Count"
        }

2. 实现会签加签通过率两种类型(个数和百分比)的全覆盖功能；  
3. 修正Gateway节点之后Transition定义的ReceiverType类型未能获取的BUG；  
4. 实现跨Gateway节点退回的功能(OrSplit单一分支撤回)。  

**Slickflow(1.5.7) 企业版更新说明:**  
1. Slickflow.Designer 设计器项目全面重构编写，更新如下：
  1). 升级到ASP.NET MVC5;
  2). 升级到BOOTSTRAP3.3.7；
  3). 图形库框架升级到JSPLUMB2.2.8，图形体验更流畅；
  4). AG-Grid取代SlickGrid，AG-Grid在开源社区方面的建设更加完善；
2. 项目解决方案VS2017版本建立。  

**Slickflow(1.5.6) 企业版更新说明：**  
1. 提供获取流程发起人的流程图连线定义（Transition property page）
2. 修正子流程节点变量名称改变后的条件判断处理；

**Slickflow(1.5.5) Demo版本功能说明：**  
**1. 引擎**  
   1). 引擎集成国产数据库人大金仓Kingbase；  
   2). 添加Slickflow.Module项目，实现组织机构的模块化构建；  
   3). 引擎实现提交至发送人员的部门主管，下属或者同级同事流转功能，相应增加部门员工数据表和存储过程；  
**2. 设计器**  
  1). 流程设计器增加节点元素添加的操作面板；  
  2). 流程设计器修正连线控制Gateway的显示Bug；  
**3. DEMO示例**  
  1). WebDemo/MvcDemo/Designer去除多项目引用，调试运行不依赖IIS Server。  

**Slickflow(1.5.2) Demo版本功能说明：**  
**1. DEMO示例**  
重新改版MvcDemo项目(电商生产订单流程)，采用Bootstrap框架，增加人员弹框功能演示；  
**2. 设计器**  
重新改版设计器项目，使用Bootstrap框架，优化界面及性能；  
**3. 引擎**  
1). 引擎增加辅助查询步骤角色用户关系接口；  
GetNextActivityRoleUserTree();  	//下一步选人弹框控件使用  
GetRoleUserListByProcess();  
GetUserListByRole();  
GetRoleUserByRoleIDs();  

**Slickflow(1.5.1) Demo版本功能说明：**  
**1. 流程引擎**  
   1). 会签加签不同模式处理，串行并行及通过率设置；会签加签内部撤销退回处理；   
   2). 引擎响应外部接口，并实现调用功能；  
**2. 设计器**  
   1). IE8及以上, Firefox 和谷歌浏览器兼容版本实现。  
   2). 增加会签加签子流程等特性配置；  
   3). 增加显示网格功能。  
**3. Slickflow多数据库支持**  
   改造Dapper，使得Slickflow支持Oracle，MySQL等数据库。  
**4. 会签加签事件交互说明文档**  
   Slickflow会签加签事件程序调用说明文档.docx  
**5. 增加Slickflow.Data项目，开放源代码**  
**6. 修正1.5.0版本对Demo中的SQL语句报错问题**  

**EMail: william.ligong@yahoo.com**
**QQ(Author): 47743901**

**Slickflow 网站:**  
http://www.slickflow.com  
**DEMO:**  
http://www.slickflow.com/demo/index  
**文档:**  
http://www.slickflow.com/wiki/index  
**捐赠:**  
http://www.slickflow.com/donate/index  
