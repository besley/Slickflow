## Slickflow
**Current Version:NET8**

![](https://img.shields.io/github/stars/besley/slickflow.svg) 
![](https://img.shields.io/github/forks/besley/slickflow.svg) 
![](https://img.shields.io/github/tag/besley/slickflow.svg) 
![](https://img.shields.io/github/release/besley/slickflow.svg) 
![](https://img.shields.io/nuget/dt/Slickflow.Engine.svg) 
![](https://img.shields.io/github/issues/besley/slickflow.svg) 


**A Quick Design and Testing Demo:**  
![And Split Demo](https://github.com/besley/besley.github.io/blob/master/Gif/slickflow-andsplit-demo.gif)

**Quick Start Tutorial for Designer Project:**  
1). In the command console, using the command **npm install** to download the node package.

   please notice to run the command, the directory location is in the **ClientApp** path of the **sfd** project.

2). Set up the **sfdapi** project which is an asp.net webapi type project.(IIS is a choice)

3). Setting webapi variable in the **kcofnig.js** file

    kconfig.webApiUrl = "http://localhost/sfdapic/" //your sfd webapi backend service url

4). In the command console, using the command **npm run dev** to the the project

5). Access the web project in the browser by 

    **http://localhost:5000**

**.NET/.NETCore Workflow Engine With Full Source Code** 
1. **.NET, .NET CORE version both supported**  
 Slickflow is an open-source project based on .NET6; It's easy to use engine product into the cross-platform application.  
2. **BPMN graphic style process diagram**   
 Slickflow is using BPMN2 notation to describe process diagram, the Slickflow designer is HTML5 graph editor and user-friendly to business process communication and business analysis.  
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

**Document:**  
http://doc.slickflow.net  
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
**Modeler Start Tutorial:**  
https://github.com/besley/Slickflow/wiki/Slickflow-Quick-Start-Tutorial  
**Modeler Demo:**  
http://demo.slickflow.com/sfd/model  
**Quasar Form Builder**  
The online dynamic form demo:http://demo.slickflow.com/sqd/   
The SlickQua project:http://github.com/besley/slickqua/   

![AskForLeave Form Approval](https://github.com/besley/besley.github.io/blob/master/Gif/SlickQua-Ask4Leave-Demo.gif)  

**Slickflow(2.0.0.0) 企业版：** 


1. 集成BpmnJS 设计器，XML模式转换为BPMN2;
2. 重构Slickflow.Engine项目;
3. 重写ProcessModelBPMN，适应BPMN2模型;
4. 全部项目 .NET8 版本实现；
5. 技术开发文档网站发布
	http://doc.slickflow.com (中文) 
	http://doc.slickflow.net(English) 
	

引擎功能各个版本描述见产品页面：

http://www.slickflow.com/product/index 


**EMail: sales@ruochisoft.com**  
**QQ(Author): 47743901**  
**WeChat(Author): besley2008**  


**Slickflow 网站:**  
http://www.slickflow.com  (中文) 
http://www.slickflow.net (English) 

**DEMO:**  
http://www.slickflow.com/demo/index  

**文档:**  
http://doc.slickflow.com (中文) 
http://doc.slickflow.net(English) 

[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](william.ligong@yahoo.com)
