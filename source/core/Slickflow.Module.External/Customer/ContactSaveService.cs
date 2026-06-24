using System;
using Newtonsoft.Json;
using Slickflow.Engine.Common;
using Slickflow.Engine.External;
using Slickflow.Module.External.Customer.Entity;
using Slickflow.Module.External.Data;

namespace Slickflow.Module.External.Customer
{
    /// <summary>
    /// Reads Customer entity from process variable "customer" (JSON), then inserts or updates biz_customer in Supabase.
    /// Sets process variable "customer_id" after save. Run CustomerExtractService first to populate "customer".
    /// </summary>
    public class ContactSaveService : ExternalServiceBase, IExternalService
    {
        public override void Execute()
        {
            if (EventService == null)
            {
                System.Diagnostics.Debug.WriteLine("CustomerSaveService: EventService is null, skip.");
                return;
            }

            var customerJson = EventService.GetVariable(ProcessVariableScopeEnum.Process, "customer");
            if (string.IsNullOrWhiteSpace(customerJson))
            {
                System.Diagnostics.Debug.WriteLine("CustomerSaveService: process variable 'customer' is empty, skip.");
                return;
            }

            var industryIdVar = EventService.GetVariable(ProcessVariableScopeEnum.Process, "industry_id");
            long? industryId = null;
            if (!string.IsNullOrWhiteSpace(industryIdVar) && long.TryParse(industryIdVar.Trim(), out var parsedId))
                industryId = parsedId;

            BizCustomerEntity entity;
            try
            {
                entity = JsonConvert.DeserializeObject<BizCustomerEntity>(customerJson);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CustomerSaveService: failed to deserialize 'customer': {ex.Message}");
                return;
            }

            if (entity == null)
            {
                System.Diagnostics.Debug.WriteLine("CustomerSaveService: deserialized customer is null, skip.");
                return;
            }

            if (industryId.HasValue)
                entity.IndustryId = industryId;

            try
            {
                var repo = new SupabaseCustomerRepository();

                // Prefer explicit customer_id from workflow variables (e.g. browser session-bound id)
                var customerIdVar = EventService.GetVariable(ProcessVariableScopeEnum.Process, "customer_id");
                BizCustomerEntity existing = null;

                if (!string.IsNullOrWhiteSpace(customerIdVar))
                {
                    existing = repo.GetByIdAsync(customerIdVar.Trim()).GetAwaiter().GetResult();
                    if (existing != null)
                    {
                        // Update existing row by customer_id
                        entity.CustomerId = existing.CustomerId;
                        repo.UpdateAsync(existing.CustomerId, entity).GetAwaiter().GetResult();
                        SaveVariableIfSupported("customer_id", existing.CustomerId);
                        System.Diagnostics.Debug.WriteLine($"CustomerSaveService: customer updated by id, customer_id={existing.CustomerId}");
                        return;
                    }

                    // No row yet for this id – create new row with this customer_id so it stays stable for this browser/session
                    entity.CustomerId = customerIdVar.Trim();
                    repo.InsertAsync(entity).GetAwaiter().GetResult();
                    SaveVariableIfSupported("customer_id", entity.CustomerId);
                    System.Diagnostics.Debug.WriteLine($"CustomerSaveService: new customer created with provided id, customer_id={entity.CustomerId}");
                    return;
                }

                // Fallback: no explicit customer_id; dedupe by wechat / phone
                existing = repo.GetByWechatOrPhoneAsync(entity.Wechat, entity.PhoneNumber ?? entity.Mobile).GetAwaiter().GetResult();

                if (existing == null)
                {
                    repo.InsertAsync(entity).GetAwaiter().GetResult();
                    SaveVariableIfSupported("customer_id", entity.CustomerId);
                    System.Diagnostics.Debug.WriteLine($"CustomerSaveService: new customer created, customer_id={entity.CustomerId}");
                }
                else
                {
                    entity.CustomerId = existing.CustomerId;
                    repo.UpdateAsync(existing.CustomerId, entity).GetAwaiter().GetResult();
                    SaveVariableIfSupported("customer_id", existing.CustomerId);
                    System.Diagnostics.Debug.WriteLine($"CustomerSaveService: customer updated by wechat/phone, customer_id={existing.CustomerId}");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogHelper.RecordException("CustomerSaveService", ex, requestData: customerJson?.Length > 500 ? customerJson.Substring(0, 500) + "..." : customerJson);
                throw;
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
                // Some runtimes may not support writing variables; ignore
            }
        }
    }
}
