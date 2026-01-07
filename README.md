# Slickflow

**Current Version: NET8**

![](https://img.shields.io/github/stars/besley/slickflow.svg) 
![](https://img.shields.io/github/forks/besley/slickflow.svg) 
![](https://img.shields.io/github/tag/besley/slickflow.svg) 
![](https://img.shields.io/github/release/besley/slickflow.svg) 
![](https://img.shields.io/nuget/dt/Slickflow.Engine.svg) 
![](https://img.shields.io/github/issues/besley/slickflow.svg) 

## Overview

**SlickFlow: Intelligent Workflow Automation with Large Language Models**

SlickFlow now integrates cutting-edge Large Language Model (LLM) nodes, empowering your workflows with advanced conversational reasoning and intelligent automation capabilities. This enhancement transforms traditional workflow systems into dynamic, AI-driven orchestration platforms.

## 🚀 Key Features

### 🤖 Native LLM Node Integration

Seamlessly incorporate LLM nodes directly into your workflow diagrams. Configure and connect AI-powered steps alongside traditional business logic for end-to-end intelligent automation.

### 🔌 Multi-LLM Provider Support

Flexible integration with leading AI services:

- **OpenAI API** (GPT-4, GPT-3.5, and beyond)
- **QianWen** (Alibaba's large language model)
- Extensible architecture for additional providers

### Image Classification & Analysis

Upload and process images directly through LLM nodes

### Retrieval-Augmented Generation (RAG)

Enhance LLM responses with real-time data retrieval from your knowledge bases

## 📚 AI Features Description
![Slickflow.AI Document](https://medium.com/@slickflow/ai-large-language-model-integration-in-slickflow-net-7a75a069aa3a)

## 📸 Demo

**AI Image Classification Process Demo:**  
![AI Image Classification Demo](https://github.com/besley/besley.github.io/blob/master/Gif/slickflow-ai-image-classification.gif)

## 🏃 Quick Start Tutorial for Designer Project

1. In the command console, using the command **npm install** to download the node package.

   **Note:** Please run the command in the **ClientApp** path of the **sfd** project.

2. Set up the **sfdapi** project which is an asp.net webapi type project. (IIS is a choice)

3. Setting webapi variable in the **kconfig.js** file

   ```javascript
   kconfig.webApiUrl = "http://localhost/sfdapic/" //your sfd webapi backend service url
   ```

4. In the command console, using the command **npm run dev** to start the project

5. Access the web project in the browser by **http://localhost:5000**

## 💻 .NET8 Workflow Engine With Full Source Code

### 0. Deepseek/QWen-Plus AI Service Supported

Slickflow can use AI deepseek service to generate BPMN flow chart through text description by users. Large model nodes also participate in business processes as intelligent nodes in the process sequence, completing the functions of intelligent generation and intelligent decision-making.

### 1. .NET, Cross Platform Development

Slickflow is an open-source project based on .NET8; It's easy to use engine product into the cross-platform application, such as Linux, MacOS.

### 2. BPMN Graphic Style Process Diagram

Slickflow is using BPMN2 notation to describe process diagram, the Slickflow designer is HTML5 graph editor and user-friendly to business process communication and business analysis.

### 3. High Performance with Dapper.NET Library

Dapper is a simple object mapper for .NET and owns the title of King of Micro ORM in terms of speed and is virtually as fast as using a raw ADO.NET data reader. An ORM is an Object Relational Mapper, which is responsible for mapping between database and programming language.

(Ref: https://dapper-tutorial.net/dapper)

### 4. Multiple Databases Supported

Slickflow supports SQLSERVER, ORACLE, MySQL and another database, it implemented by the Dapper.NET extension library. The .net core version using EF core to support different database products.

### 5. Workflow Patterns Supported

![Workflow Pattern](http://www.slickflow.com/content/img/sfterm-en.png)

#### 1). Sequence

The most frequently process pattern

#### 2). Split/Merge

Support and/or gateway such as **and/or split**, **and/or join**, together with condition variables on the transition

#### 3). Sub-process

In the main process, a subprocess node can start a new process life cycle.

#### 4). Multi-instance

Multiple performers processing a task together by multiple task instances. All performers both compete for their task, then the process can be continued on. There are **sequence** and **parallel** pattern, and the **percentage** or **count** parameters can be set on it to ensure when can go to the next step.

![Multiple Instance Pattern](http://www.slickflow.com/content/img/wfpattern-mi-en.png)

#### 5). Event Interoperation

Process instance and activity instance event delegation service, such as process/activity start, execute and complete.

#### 6). Timer

Integrated with **HangFire** library, and with **CRON** expression supported

#### 7). Email

Todo or overdue tasks email notification

#### 8). Withdraw

Withdraw the task after just sent out to next step users.

#### 9). Sendback

Send back to the previous step user, because of some exceptions.

#### 10). Resend

Combined after sendback and re-send the task to original sendback users.

#### 11). Reverse

Reverse the process instance alive when completed.

#### 12). Jump

Jump the process over by several steps forward or backward.

#### 13). MessageQueue (RabbitMQ)

Message publishing and subscribing to implement message throwing and catching.

### 6. Process Version

The process has version property to upgrade a new definition due to the business process changed.

### 7. XML Cache

The runtime instance use cache to keep the XML process diagram by an expired duration.

### 8. Sequence Process Code Style

#### 0). Model

Create a simple sequence process diagram by hand code rather than a HTML designer:

```csharp
//create a simple sequence process diagram by hand code rather than a HTML designer  
var pmb = ProcessModelBuilder.CreateProcess("simple-process-name", "simple-process-code");
var process = pmb.Start("Start")
    .Task("Task1")
    .Task("Task2")
    .End("End")
    .Store();
```

![simple sequence diagram](http://www.slickflow.com/content/img/simple-sequence.png)

#### 1). Start

Start a new process instance:

```csharp
//start a new process instance
IWorkflowService wfService = new WorkflowService();
var wfResult = wfService.CreateRunner("10", "jack")
            .UseApp("DS-100", "Book-Order", "DS-100-LX")
            .UseProcess("PriceProcessCode")
            .Start();
```

#### 2). Run

Run a process instance to next step:

```csharp
//run a process instance to next step
IWorkflowService wfService = new WorkflowService();
var wfResult = wfService.CreateRunner("10", "jack")
            .UseApp("DS-100", "Book-Order", "DS-100-LX")
            .UseProcess("PriceProcessCode")
            .NextStepInt("20", "Alice")
            .Run();
```

#### 3). Withdraw

Withdraw a activity instance to previous step:

```csharp
//Withdraw a activity instance to previous step
IWorkflowService wfService = new WorkflowService();
var wfResult = wfService.CreateRunner("10", "Jack")
            .UseApp("DS-100", "Book-Order", "DS-100-LX")
            .UseProcess("PriceProcessCode")
            .OnTask(id)             //TaskID
            .Withdraw();
```

#### 4). SendBack

Sendback a activity instance to previous step:

```csharp
//Sendback a activity instance to previous step
IWorkflowService wfService = new WorkflowService();
var wfResult = wfService.CreateRunner("20", "Alice")
            .UseApp("DS-100", "Book-Order", "DS-100-LX")
            .UseProcess("PriceProcessCode")
            .PrevStepInt()
            .OnTask(id)             //TaskID
            .SendBack();
```

### 9. Rich Demo Projects

WebDemo, MvcDemo, and WinformDemo project are demonstrated for a different type of enterprise information systems.

### 10. Target

Slickflow is very suitable for software teams or companies who want to integrate workflow engine into their products.

### 11. Suggestions

Slickflow is suggested to give programmers a flexible way to integrate workflow engine components into their products or customer projects. The programmers can write their own code segments based on the engine component.

### 12. Open Source Project License

Slickflow follows MIT open source protocol and can be used for commercial purposes.

### 13. Technical Support

The enterprise, ultimate and universe version can be provided with a technical support and upgrade service.

If you have any further inquiry, please feel free to contact us with:

**Email:** support@ruochisoft.com

## 📚 Resources

### Document

- **English:** http://doc.slickflow.net
- **中文:** http://doc.slickflow.com

### Wiki Page

https://github.com/besley/Slickflow/wiki

### CodeProject Articles

- https://www.codeproject.com/Articles/5246528/Slickflow-NET-Core-Open-Source-Workflow-Engine
- https://www.codeproject.com/Articles/5252483/Slickflow-Coding-Graphic-Model-User-Manual

### Slickflow Website

- **English:** http://www.slickflow.net
- **中文:** http://www.slickflow.com

### Demo

- **Demo:** http://www.slickflow.com/demo/index
- **Designer Demo:** http://demo.slickflow.com/sfd/

### Docker Hub

Pre-built Docker images are available on Docker Hub. Get started in minutes without building from source!

#### All-in-One Image (Recommended)

The easiest way to get started - pull one image and run all three services:

```bash
docker pull besley2096/slickflow-all:latest
docker run -d \
  -p 5000:5000 \
  -p 5001:5001 \
  -p 8090:8090 \
  -e WfDBConnectionType=PGSQL \
  -e WfDBConnectionString="Server=host.docker.internal;Port=5432;Database=wfdbbpmn2;User Id=postgres;Password=your-password;TimeZone=UTC;" \
  --name slickflow-all \
  besley2096/slickflow-all:latest
```

**Access:**
- **Frontend Designer**: http://localhost:8090
- **Backend API**: http://localhost:5000
- **WebTest**: http://localhost:5001

**Docker Hub:** https://hub.docker.com/r/besley2096/slickflow-all

**Advantages:**
- Single Image: Pull one image instead of three
- Simplified Deployment: Run one container instead of three
- Resource Efficient: Shared base image and dependencies
- Easy to Use: Single command to start all services

#### Separate Images

For better isolation and scaling, use separate images:

**Backend API:**
```bash
docker pull besley2096/slickflow-api:latest
docker run -d -p 5000:5000 \
  -e WfDBConnectionType=PGSQL \
  -e WfDBConnectionString="Server=host.docker.internal;Port=5432;Database=wfdbbpmn2;User Id=postgres;Password=your-password;TimeZone=UTC;" \
  --name slickflow-api \
  besley2096/slickflow-api:latest
```

**Frontend Designer:**
```bash
docker pull besley2096/slickflow-designer:latest
docker run -d -p 8090:8090 \
  --name slickflow-designer \
  besley2096/slickflow-designer:latest
```

**WebTest:**
```bash
docker pull besley2096/slickflow-webtest:latest
docker run -d -p 5001:5001 \
  -e WfDBConnectionType=PGSQL \
  -e WfDBConnectionString="Server=host.docker.internal;Port=5432;Database=wfdbbpmn2;User Id=postgres;Password=your-password;TimeZone=UTC;" \
  --name slickflow-webtest \
  besley2096/slickflow-webtest:latest
```

#### Docker Compose

Use `docker-compose.hub.yml` from this repository:

```bash
docker-compose -f docker-compose.hub.yml pull
docker-compose -f docker-compose.hub.yml up -d
```

#### Image Tags

Available tags for each image:
- `latest` - Latest stable version
- `v3.5.0` - Version 3.5.0

#### Docker Hub Links

- [slickflow-all](https://hub.docker.com/r/besley2096/slickflow-all) - All-in-one image (Recommended)
- [slickflow-api](https://hub.docker.com/r/besley2096/slickflow-api) - Backend API only
- [slickflow-designer](https://hub.docker.com/r/besley2096/slickflow-designer) - Frontend designer only
- [slickflow-webtest](https://hub.docker.com/r/besley2096/slickflow-webtest) - WebTest only

#### Database Configuration

API and WebTest containers require database configuration. Supported databases: PostgreSQL, MySQL, SQL Server, Oracle.

**Database on Host Machine:**
```bash
-e WfDBConnectionString="Server=host.docker.internal;Port=5432;Database=wfdbbpmn2;User Id=postgres;Password=your-password;TimeZone=UTC;"
```

**Database in Docker Container:**
```bash
-e WfDBConnectionString="Server=postgres;Port=5432;Database=wfdbbpmn2;User Id=postgres;Password=your-password;TimeZone=UTC;"
```

**Remote Database:**
```bash
-e WfDBConnectionString="Server=192.168.1.100;Port=5432;Database=wfdbbpmn2;User Id=postgres;Password=your-password;TimeZone=UTC;"
```

## 📞 Contact

- **Email:** sales@ruochisoft.com
- **QQ (Author):** 47743901
- **WeChat (Author):** besley2008

## 💰 Donation

您的捐赠将用于产品的持续研发和社区建设

Your donation will be used for the continuous research and development of the product and community building

[![Donate with PayPal](https://github.com/besley/besley.github.io/blob/master/Images/paypal/donation.png)](https://paypal.me/slickflownet)

