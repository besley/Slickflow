# Slickflow.Module.External.Tests

Console test for **CustomerService** and **MessageService**: simulates a customer–AI chat, extracts contact info, and saves to Supabase (`biz_customer`, `biz_conversation`).

## What it does

1. Sets process variables:
   - **user_message**: Simulated customer message (Chinese) containing name (张三), mobile (13812345678), wechat (zhang_san_88), email (zhang@example.com).
   - **ai_response**: Simulated AI reply.
2. Runs **CustomerService**: parses `user_message` with regex → inserts/updates **biz_customer** in Supabase, writes **customer_id** to process variable.
3. Runs **MessageService**: saves **user_message** + **ai_response** (+ **customer_id** if set) to **biz_conversation** in Supabase.

## Prerequisites

- Supabase project with:
  - Table **biz_customer** (see plugin `Data` folder or existing schema).
  - Table **biz_conversation** (create with `Data/biz_conversation_supabase.sql`).

## Configuration

Use either:

- **appsettings.json** (copied to output):
  - `AiModelProvider:SupabaseProjectUrl`: e.g. `https://YOUR_PROJECT_REF.supabase.co`
  - `AiModelProvider:SupabaseServiceRoleKey`: your service role key (or anon key for testing)
- **Environment variables**:
  - `SUPABASE_URL` or `SUPABASE_PROJECT_URL`
  - `SUPABASE_SERVICE_ROLE_KEY` or `SUPABASE_ANON_KEY`

Copy `appsettings.Development.json` or edit `appsettings.json` with real values (do not commit real keys).

## Run

```bash
cd source/core/Slickflow.Module.External.Tests
dotnet run
```

Or run the built exe from `bin/Debug/net8.0` (ensure `appsettings.json` is in the same directory).

If Supabase is not configured, the program exits with a short message and exit code 1.
