# Slickflow Configuration Guide

## Development Environment

| Item | Requirement |
|------|-------------|
| Operating System | Windows 7 and above |
| Programming Language | C# / .NET 8 and above |
| IDE | Visual Studio 2022 and above |
| Database | PostgreSQL 15 and above |

---

## Project Setup

### 1. Slickflow Designer (sfd) — Frontend

The designer is a Vue/Webpack frontend project located at `source/sfd/`.

**Install dependencies:**
```bash
cd source/sfd/ClientApp
npm install
```

**Configure the backend API URL** in `source/sfd/ClientApp/app/kconfig.js`:
```js
kconfig.webApiUrl = "http://localhost/sfdapi/"   // your sfdapi backend URL
```

**Start the dev server:**
```bash
npm run dev
```

---

### 2. sfdapi — Backend WebAPI

`sfdapi` is an ASP.NET Core Web API project. It can be hosted via IIS or run directly with `dotnet run`.

**Configure the database connection** in `source/sfdapi/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "WfDBConnectionType": "PGSQL",
    "WfDBConnectionString": "Server=127.0.0.1;Port=5432;Database=wfdbtest2099;User Id=postgres;Password=<YOUR_DB_PASSWORD>;"
  }
}
```

Supported database types: `PGSQL` / `MYSQL` / `SQLSERVER` / `ORACLE`

---

### 3. AI Model Provider (Optional)

To enable AI-powered features (LLM / RAG / Agent nodes), configure `AiModelProvider` in `appsettings.json`:

```json
{
  "AiModelProvider": {
    "ActiveProvider": "QianWen",
    "SupabaseProjectUrl": "https://<YOUR_SUPABASE_PROJECT>.supabase.co",
    "SupabaseServiceRoleKey": "<YOUR_SUPABASE_SERVICE_ROLE_KEY>",
    "QianWen": {
      "BaseUrl": "https://dashscope.aliyuncs.com/compatible-mode",
      "ApiKey": "<YOUR_QIANWEN_API_KEY>",
      "Model": "qwen-plus"
    },
    "OpenAI": {
      "BaseUrl": "https://api.openai.com/v1",
      "ApiKey": "<YOUR_OPENAI_API_KEY>",
      "Model": "gpt-4o"
    }
  }
}
```

---

## Demo Projects

### Slickflow.MvcDemo — Order Process Demo

An ASP.NET MVC demo showing a typical order approval workflow.

Configure `source/demo/Slickflow.MvcDemo/appsettings.json` with your database connection string (same format as sfdapi above).

### Slickflow.WebDemo — Leave Process Demo

A Vue SPA demo showing an HR leave approval workflow.

- **IIS site** pointing to the `Slickflow.WebDemo` publish output
- Configure `appsettings.json` with your database connection string

---

## Database Setup

### Create the Database

Scripts are located in the `database/` directory:

| File | Purpose |
|------|---------|
| `wfdbtest2099_pgsql_schema.sql` | DDL — creates all tables, indexes, sequences |
| `wfdbtest2099_pgsql_data.sql` | DML — inserts initial seed and demo data |

**Import order:**
```bash
psql -h 127.0.0.1 -U postgres -d wfdbtest2099 -f database/wfdbtest2099_pgsql_schema.sql
psql -h 127.0.0.1 -U postgres -d wfdbtest2099 -f database/wfdbtest2099_pgsql_data.sql
```

### Core Engine Tables

| Table | Description |
|-------|-------------|
| `wf_process` | Process definition |
| `wf_process_instance` | Process instance (runtime) |
| `wf_activity_instance` | Activity instance (runtime) |
| `wf_transition_instance` | Transition/flow instance (runtime) |
| `wf_task` | Task list |
| `wf_log` | Execution log |
| `wf_variable` | Variable definitions |
| `wf_rule_set` | Business rule sets |

### AI Tables

| Table | Description |
|-------|-------------|
| `ai_model_provider` | AI provider configuration |
| `ai_activity_config` | Per-activity AI node configuration |
| `ai_agent` | Agent definitions |
| `ai_agent_parameter` | Agent tool parameters |

### System Tables

| Table | Description |
|-------|-------------|
| `sys_user` | Users |
| `sys_role` | Roles |
| `sys_department` | Departments |
| `sys_resource` | Resources / permissions |

---

## More Information

- Official website: [http://www.slickflow.com](http://www.slickflow.com)
- GitHub: [https://github.com/besley/Slickflow](https://github.com/besley/Slickflow)
