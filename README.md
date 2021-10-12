## Slickflow

![](https://img.shields.io/github/stars/besley/slickflow.svg) 
![](https://img.shields.io/github/forks/besley/slickflow.svg) 
![](https://img.shields.io/github/tag/besley/slickflow.svg) 
![](https://img.shields.io/github/release/besley/slickflow.svg) 
![](https://img.shields.io/nuget/dt/Slickflow.Engine.svg) 
![](https://img.shields.io/github/issues/besley/slickflow.svg) 


**A Quick Design and Testing Demo:**  
![Large Order Demo](https://github.com/besley/besley.github.io/blob/master/Gif/slickflow-largeorder-andsplit-demo-en.gif)

**.NET/.NETCore Workflow Engine With Full Source Code** 
1. **.NET, .NET CORE version both supported**  
 Slickflow is an open-source project based on .NET5; It's easy to use engine product into the cross-platform application.  
2. **BPMN graphic style process diagram**   
 Slickflow is using BPMN notation to describe process diagram, the Slickflow designer is HTML5 graph editor and user-friendly to business process communication and business analysis.  
3. **High performance with Dapper.NET library**  
 Dapper is a simple object mapper for .NET and owns the title of King of Micro ORM in terms of speed and is virtually as fast as using a raw ADO.NET data reader. An ORM is an Object Relational Mapper, which is responsible for mapping between database and programming language.
(Ref: https://dapper-tutorial.net/dapper)  
 4. **Multiple databases supported**  
 Slickflow supports SQLSERVER, ORACLE, MySQL and another database, it implemented by the Dapper.NET extension library. The .net core version using EF core to support different database products.  
5. **Workflow patterns supported**  
![Wokflow Pattern](http://www.slickflow.com/content/img/sfterm-en.png)  
 **1). Sequence**  
    the most frequently process pattern   
 **2). Split/Merge**  
   support and/or gateway such as **and/or split**, **and/or join**, together with  condition variables on the transition  
 **3). Sub-process**  
    in the main process, a subprocess node can start a new process life cycle.  
 **4). Multi-instance**  
   multiple performers processing a task together by multiple task instances. All performers both compete for their task, then the process can be continued on. There are **sequence** and **parallel** pattern, and the **percentage** or **count** parameters can be set on it to ensue when can go to the next step.   
    ![Muliple Instance Pattern](http://www.slickflow.com/content/img/wfpattern-mi-en.png)  
 **5). Event interoperation**  
   process instance and activity instance event delegation service, such as process/activity start, execute and complete.  
 **6). Timer**  
   integrated with **HangFire** library, and with **CRON** expression supported  
 **7). Email**  
   todo or overdue tasks email notification  
 **8). Withdraw**  
	withdraw the task after just sent out to next step users.  
 **9). Sendback**  
    send back to the previous step user, because of some exceptions.  
 **10). Resend**  
    combined after sendback and re-send the task to original sendback users.  
 **11). Reverse**  
    reverse the process instance alive when completed.  
 **12). Jump**  
    jump the process over by several steps forward or backward.   
 **13). MessageQueue(RabbitMQ)**  
    message publishing and subscribing to implement message throwing and catching. 
    
**6. Process Version**  
     the process has version property to upgrade a new definition due to the business process changed.    
**7. XML Cache**    
     the runtime instance use cache to keep the XML process diagram by an expired duration.  
**8. Sequence Process Code Style**  
 **0). Model**  
	
    //create a simple sequence process diagram by hand code rather than a HTML designer  
    var pmb = ProcessModelBuilder.CreateProcess("simple-process-name", "simple-process-code");
	var process = pmb.Start("Start")
		.Task("Task1")
		.Task("Task2")
		.End("End")
		.Store();       

   ![simple sequence diagram](http://www.slickflow.com/content/img/simple-sequence.png)  
    
                
 **1). Start**  
    
    //start a new process instance
    IWorkflowService wfService = new WorkflowService();
    var wfResult = wfService.CreateRunner("10", "jack")
                .UseApp("DS-100", "Book-Order", "DS-100-LX")
                .UseProcess("PriceProcessCode")
                .Start();

 **2). Run**  
    
    //run a process instance to next step
    IWorkflowService wfService = new WorkflowService();
    var wfResult = wfService.CreateRunner("10", "jack")
                .UseApp("DS-100", "Book-Order", "DS-100-LX")
                .UseProcess("PriceProcessCode")
                .NextStepInt("20", "Alice")
                .Run();
				 
 **3). Withdraw**  
    
    //Withdraw a activity instance to previous step
    IWorkflowService wfService = new WorkflowService();
    var wfResult = wfService.CreateRunner("10", "Jack")
                .UseApp("DS-100", "Book-Order", "DS-100-LX")
                .UseProcess("PriceProcessCode")
                .OnTask(id)             //TaskID
                .Withdraw();

 **4). SendBack**  
    
    //Sendback a activity instance to previous step
    IWorkflowService wfService = new WorkflowService();
    var wfResult = wfService.CreateRunner("20", "Alice")
                .UseApp("DS-100", "Book-Order", "DS-100-LX")
                .UseProcess("PriceProcessCode")
                .PrevStepInt()
                .OnTask(id)             //TaskID
                .SendBack();

**9. Rich demo projects**  
  WebDemo, MvcDemo, and WinformDemo project are demonstrated for a different type of enterprise information systems.   
**10. Target**  
  Slickflow is very suitable for software teams or companies who want to integrate workflow engine into their products.  
**11. Suggestions**  
  Slickflow is suggested to give programmers a flexible way to integrate workflow engine components into their products or customer projects. The programmers can write their own code segments based on the engine component.   
**12. Open Source Project License**    
 The product is under **Slickflow Open Source Project license**.    
 1). Slickflow software must be legally used, and should not be used in violation of the law, morality and other acts that endanger social interests;  
 2). Non-transferable, non-transferable and indivisible authorization of this software;  
 3). The source code can be modified to apply Slickflow components in their own projects or products, but Slickflow source code can not be separately encapsulated for sale or distributed to third-party users;  
 4). The intellectual property rights of Slickflow software shall be protected by law, and no documents such as technical data shall be made public or sold.  
**13. Commercial license**  
 The enterprise, ultimate and universe version can be provided with a commercial license, technical support and upgrade service.
 
 if you have any further inquiry, please feel free to contact us:   

**Email: sales@ruochisoft.com**  
**QQ(Author): 47743901**

**Quick Start Tutorial:**  
https://github.com/besley/Slickflow/wiki/Slickflow-Quick-Start-Tutorial  
**Wiki Page:**  
https://github.com/besley/Slickflow/wiki  
**CodeProject Articles:**  
https://www.codeproject.com/Articles/5246528/Slickflow-NET-Core-Open-Source-Workflow-Engine 
https://www.codeproject.com/Articles/5252483/Slickflow-Coding-Graphic-Model-User-Manual  
**Slickflow website:**  
http://www.slickflow.net  
http://www.slickflow.com  
**Demo:**  
http://www.slickflow.com/demo/index  
**Designer Demo:**  
http://demo.slickflow.com/sfd/  
**Modeler Demo:**  
http://demo.slickflow.com/sfd/model  
**Document:**  
http://www.slickflow.com/wiki/index  
**Quasar Form Builder**  
The online dynamic form demo:http://demo.slickflow.com/sqd/   
The SlickQua project:http://github.com/besley/slickqua/   

![AskForLeave Form Approval](https://github.com/besley/besley.github.io/blob/master/Gif/SlickQua-Ask4Leave-Demo.gif)  

**Slickflow(1.7.7.0) 企业版：** 

2021-09-28
1. 引擎增加三种类型的中间事件(Intermediate)处理节点定时、消息和条件；
2. 重新调整HangFire作业功能，检测流程实例活动实例的JobTimer数据；
3. 简化并合并语言本地化项目，只保留Engine, Desiger和Web三种项目类型；

2021-07-18
1. 完成动态表单设计器Quasar版，并发版；
2. 完成Quasar版本的多语言及提示组件配置和开发；
3. 完成新版本WebDemo请假项目的开发，测试并发布；

2021-04-29
1. 引擎产品升级到.NET5版本；
2. Quasar(VUE)请假流程Demo示例；
3. 实现会签结合审批网关分支流转模式；
4. 设计器及测试工具多语言(中英文)界面显示；
5. 增加pgsql脚本，满足用户数据库需求；


2021-02-26
1. 动态子流程调用功能实现；
2. 增加会签通过率的审批网关（ApprovalOrSplit）模式；
3. 引擎项目升级到.netcore3.1版本，asp.net core 5版本；
4. 增加西班牙语(Spanish)语言本地化功能；

2020-11-11
1. 新增加自动服务节点(ServiceTask)；
2. 新增加审批网关类型(ApprovalOrSplit)；
3. 增加或分支(OrSplit)条件都不满足时的默认分支选项；
4. 新增表单版本功能；

2020-09-14
1. 完成表单控件数据源绑定功能；
2. 完成表单数据控件级联事件处理；
3. 完成表单控件基本事件响应功能；
4. 修正表单绑定流程节点权限操作功能；

2020-07-30
1. 流程设计器Linux版本的路径大小写问题解决；
2. 跨流程消息交互功能完整测试通过；
3. 流程各个模块项目多语言功能完整实现；

2020-05-11
1. 中间消息节点Throw/Catch功能定义实现;
2. 开始消息启动节点：消息发布订阅启动流程实现;
3. 主流程和泳道流程的版本管理、更新和删除管理；

2020-04-02
1. 集成RabbitMQ消息队列;
2. 实现多语言(中文\英文)环境配置项目;
3. 实现多泳道流程图保存功能；

2020-01-08 
1. 开发代码图形建模工具Slickflow.Graph工具；
2. 集成CodeMirror实现引擎建模语言脚本执行；
3. 实现流程有效性验证功能；
4. 增加节点外部事件类型(C#组件方法反射调用)；
5. 增加中间事件节点(IntermediateEvent)的事件注册和调用
6. 搭建面向全球客户引擎论坛:Http://forum.slickflow.com；

2019-09-19
1. 增加外部事件类型
1). WebApi方法调用；
2). SQL脚本和SQL存储过程调用；
3). Python脚本调用

2. 重构撤销(Withdraw)功能；
3. 流程变量修改为区分和支持流程和活动类型；
4. WebTest测试产品增加流程变量的存储和显示操作；
5. 数据库WfProcessVariable表字段变化：
1). 增加ProcessGUID, ActivityGUID和ActivityName字段；
2). 删除ActivityInstanceID字段。
   
2019-05-05
1. 增加互斥(XOr)分支和合并模式；
2. 增加增强或合并模式(EOrJoin)，用于解决强制分支或者合并数目限制的用户需求
3. 合并节点之后紧跟分支的条件判断功能增加；
4. 并行多实例分支内部的退回处理，多路并行需要找到对应的节点办理人员信息；
5. 增加UpgradeProcess()接口，用于流程记录版本升级的基础接口；
6. WebTest测试项目功能增加条件变量和控制参数。

2019-04-15
1. 增加并行分支节点退回；
2. 增加结束节点的事件绑定；
3. 增加“驳回”和“办结”接口方法
   详细说明请参考高级开发技术文档

2019-03-25
1. 整合Activity属性配置页面到Activity/Index；
2. 增加节点自定义属性MyProperties的属性读取（Json）；
3. 删除原ActivityInstance的ActivityUrl字段，统一从Activity的xml内容读取；

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

**EMail: sales@ruochisoft.com**  
**QQ(Author): 47743901**

**Slickflow 网站:**  
http://www.slickflow.com  
**DEMO:**  
http://www.slickflow.com/demo/index  
**文档:**  
http://www.slickflow.com/wiki/index  

[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](william.ligong@yahoo.com)
