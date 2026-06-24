using System;
using Newtonsoft.Json;
using Slickflow.Engine.Common;
using Slickflow.Engine.External;
using Slickflow.Module.External.Customer.Entity;
using Slickflow.Module.External.Data;

namespace Slickflow.Module.External.Customer
{
    /// <summary>
    /// Loads customer from biz_customer (Supabase) by customer_id and writes it into workflow context variables.
    /// Intended to run at the start of a multi-turn flow so that RAG/LLM nodes and downstream services
    /// see the latest customer (name, wechat, mobile, etc.) and avoid repeating questions.
    /// </summary>
    public class CustomerLoadService : ExternalServiceBase, IExternalService
    {
        public override void Execute()
        {
            if (EventService == null)
            {
                System.Diagnostics.Debug.WriteLine("CustomerLoadService: EventService is null, skip.");
                return;
            }

            var customerId = EventService.GetVariable(ProcessVariableScopeEnum.Process, "customer_id");
            if (string.IsNullOrWhiteSpace(customerId))
            {
                System.Diagnostics.Debug.WriteLine("CustomerLoadService: customer_id is empty, skip.");
                return;
            }

            try
            {
                var repo = new SupabaseCustomerRepository();
                var entity = repo.GetByIdAsync(customerId.Trim()).GetAwaiter().GetResult();
                if (entity == null)
                {
                    System.Diagnostics.Debug.WriteLine($"CustomerLoadService: no row for customer_id={customerId}, skip.");
                    return;
                }

                var json = JsonConvert.SerializeObject(entity);
                SaveVariableIfSupported("customer", json);
                SaveVariableIfSupported("customer_id", entity.CustomerId ?? customerId.Trim());
                System.Diagnostics.Debug.WriteLine($"CustomerLoadService: loaded customer into context, customer_id={entity.CustomerId}");
            }
            catch (Exception ex)
            {
                // Do not break workflow when Supabase is unavailable or query fails.
                System.Diagnostics.Debug.WriteLine($"CustomerLoadService: failed to load customer: {ex.Message}");
            }
        }

        private void SaveVariableIfSupported(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name) || value == null) return;
            try
            {
                EventService?.SaveVariable(ProcessVariableScopeEnum.Process, name, value);
            }
            catch
            {
                // Some runtimes may not support writing variables; ignore.
            }
        }
    }
}
