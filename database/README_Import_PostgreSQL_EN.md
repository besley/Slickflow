## How to Import the Slickflow Database (PostgreSQL)

This guide explains how customers can restore the Slickflow demo / sample database using the SQL files in this `database` folder.

- **Schema file** (database structure only):  
  `wfdbbpmn2_pgsql_schema.sql`

- **Data file** (table data only, no sensitive API keys):  
  `wfdbbpmn2_pgsql_data.sql`  
  Records from `ai_model_provider` and `ai_activity_config` are **intentionally excluded** to avoid leaking API keys or other secrets.

---

### 1. Prerequisites

1. PostgreSQL is installed, and the `psql` command-line tool is available in your PATH.
2. You have a PostgreSQL user (for example `postgres`) and password.
3. A target database exists for Slickflow (for example `wfdbbpmn2`).

You can create the database with:

```bash
createdb -h 127.0.0.1 -p 5432 -U postgres wfdbbpmn2
```

Adjust host, port, user, and database name as needed for your environment.

---

### 2. Import the Schema (Structure)

From this `database` directory, run:

```bash
psql "postgresql://postgres:YOUR_PASSWORD@127.0.0.1:5432/wfdbbpmn2?sslmode=disable" -f wfdbbpmn2_pgsql_schema.sql
```

Replace:

- `YOUR_PASSWORD` with your actual PostgreSQL password.
- `wfdbbpmn2` with your target database name if different.

This will create all tables, views, sequences, indexes, and comments needed by Slickflow.

---

### 3. Import the Data (Sample Records)

After the schema is imported, run:

```bash
psql "postgresql://postgres:YOUR_PASSWORD@127.0.0.1:5432/wfdbbpmn2?sslmode=disable" -f wfdbbpmn2_pgsql_data.sql
```

This will insert the sample data for the workflow engine and demo applications.

Notes:

- Data for `ai_model_provider` and `ai_activity_config` **is not included** in this file to protect API keys and other sensitive configuration.
- You can insert your own AI model configuration records later in your own environment.

---

### 4. Windows PowerShell Example

If you place this `database` folder at `C:\slickflow\database`, you can run:

```powershell
cd C:\slickflow\database

# Import schema
psql "postgresql://postgres:123456@127.0.0.1:5432/wfdbbpmn2?sslmode=disable" -f .\wfdbbpmn2_pgsql_schema.sql

# Import data
psql "postgresql://postgres:123456@127.0.0.1:5432/wfdbbpmn2?sslmode=disable" -f .\wfdbbpmn2_pgsql_data.sql
```

Change the password, host, port, and database name as needed.

After these two steps, the Slickflow database should be ready for use in your local environment.

