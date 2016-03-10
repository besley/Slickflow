# Slickflow
.NET Workflow Engine

Slickflow is implemented by activity and transition iterative algorithm. It supports sequence, split/merge, sub-process, multi-instance, withdraw, sendback and reverse pattern.

Slickflow used BPMN graphic elements to build its workflow natation.

Slickflow designer is a HTML5, JQuery, SVG based web designer.

Slickflow project support SQLSERVER, ORACLE, MySQL database, it implemented by dapper library.

There are demo programs that you can find in WebDemo, WinformDemo project.

The source project is under LGPL license, we also provide customers commercial license. if you have any further inquery, please feel free to contact us: 


QQ(Author): 47743901

Slickflow website:
http://www.slickflow.com

Demo:
http://www.slickflow.com/demo/index

Document:
http://www.slickflow.com/wiki/index


### Slickflow(1.5.5) Demo版本功能说明：
**1. 引擎**

- 1) 引擎集成国产数据库人大金仓Kingbase；

- 2) 添加Slickflow.Module项目，实现组织机构的模块化构建；

- 3) 引擎实现提交至发送人员的部门主管，下属或者同级同事流转功能，相应增加部门员工数据表和存储过程；

  <Transition>
    ...
    <Receiver type="Superior" />
    ...
  </Transition>
  
**2. 设计器**

- 1) 流程设计器增加节点元素添加的操作面板；

- 2) 流程设计器修正连线控制Gateway的显示Bug；

**3. DEMO示例**

- 1) WebDemo/MvcDemo/Designer去除多项目引用，调试运行不依赖IIS Server。


### Slickflow(1.5.2) Demo版本功能说明：
**1. DEMO示例**

重新改版MvcDemo项目(电商生产订单流程)，采用Bootstrap框架，增加人员弹框功能演示；

**2. 设计器**

重新改版设计器项目，使用Bootstrap框架，优化界面及性能；

**3. 引擎**

- 1）引擎增加辅助查询步骤角色用户关系接口；

GetNextActivityRoleUserTree();  	//下一步选人弹框控件使用

GetRoleUserListByProcess();

GetUserListByRole();

GetRoleUserByRoleIDs();



### Slickflow(1.5.1) Demo版本功能说明：

**1. 流程引擎**

  - 1) 会签加签不同模式处理，串行并行及通过率设置；会签加签内部撤销退回处理；
  - 2) 引擎响应外部接口，并实现调用功能；

**2. 设计器**

  - 1) IE8及以上, Firefox 和谷歌浏览器兼容版本实现。
  - 2) 增加会签加签子流程等特性配置；
  - 3) 增加显示网格功能。

**3. Slickflow多数据库支持**

  - 改造Dapper，使得Slickflow支持Oracle，MySQL等数据库。

**4. 会签加签事件交互说明文档**

  - Slickflow会签加签事件程序调用说明文档.docx

**5. 增加Slickflow.Data项目，开放源代码**
  
**6. 修正1.5.0版本对Demo中的SQL语句报错问题**


EMail: william.ligong@yahoo.com

QQ(Author): 47743901

Slickflow 网站:

http://www.slickflow.com

DEMO:

http://www.slickflow.com/demo/index

文档:

http://www.slickflow.com/wiki/index

捐赠:

http://www.slickflow.com/donate/index
