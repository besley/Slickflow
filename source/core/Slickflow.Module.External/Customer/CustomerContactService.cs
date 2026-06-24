using System;
using Newtonsoft.Json;
using Slickflow.Engine.Common;
using Slickflow.Engine.External;
using Slickflow.Module.External.Customer.Entity;
using Slickflow.Module.External.Data;
using Slickflow.Module.External.Utility;

namespace Slickflow.Module.External.Customer
{
    /// <summary>
    /// Customer contact service for EV RAG workflows.
    /// 
    /// Responsibilities (Execute):
    /// 1) Parse contact_json (LLM JSON parser output) into contact fields.
    /// 2) Save / update customer contact info into Supabase biz_customer.
    /// 3) Output BizCustomerEntity JSON to process variable "customer" and ensure "customer_id" is set.
    /// 
    /// JSON schema for contact_json:
    /// {
    ///   "name": string | null,
    ///   "mobile": string | null,
    ///   "phone_number": string | null,
    ///   "wechat": string | null,
    ///   "email": string | null
    /// }
    /// </summary>
    public class CustomerContactService : ExternalServiceBase, IExternalService
    {
        /// <summary>
        /// Lightweight history DTO used to append contact-summary messages into chat_history JSON.
        /// Matches the shape expected by ChatHistoryMessage: { "role": "...", "content": "..." }.
        /// </summary>
        private class HistoryItem
        {
            [JsonProperty("role")]
            public string Role { get; set; }

            [JsonProperty("content")]
            public string Content { get; set; }
        }

        /// <summary>
        /// DTO for contact_json coming from the LLM JSON parser.
        /// </summary>
        private class ContactJsonDto
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("mobile")]
            public string Mobile { get; set; }

            [JsonProperty("phone_number")]
            public string PhoneNumber { get; set; }

            [JsonProperty("wechat")]
            public string Wechat { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }
        }

        public override void Execute()
        {
            if (EventService == null)
            {
                System.Diagnostics.Debug.WriteLine("CustomerContactService: EventService is null, skip.");
                return;
            }

            // 1. Parse contact_json into ContactInfo
            var contactJson = EventService.GetVariable(ProcessVariableScopeEnum.Process, "contact_json");
            if (string.IsNullOrWhiteSpace(contactJson))
            {
                System.Diagnostics.Debug.WriteLine("CustomerContactService: contact_json is empty or missing, skip.");
                return;
            }

            ContactInfo contact = null;
            try
            {
                var dto = JsonConvert.DeserializeObject<ContactJsonDto>(contactJson);
                if (dto != null)
                {
                    contact = new ContactInfo
                    {
                        Name = dto.Name,
                        Mobile = dto.Mobile,
                        Telephone = dto.PhoneNumber,
                        Wechat = dto.Wechat,
                        Email = dto.Email
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CustomerContactService: failed to deserialize contact_json: {ex.Message}");
                return;
            }

            if (!HasAnyContactField(contact))
            {
                System.Diagnostics.Debug.WriteLine("CustomerContactService: contact_json has no usable contact fields, skip.");
                return;
            }

            // 2. Build BizCustomerEntity (prefer existing customer_id from workflow variables)
            var existingCustomerId = EventService.GetVariable(ProcessVariableScopeEnum.Process, "customer_id");
            var entity = BuildBizCustomerFromContact(contact, existingCustomerId);

            // 2a. Set industry_id from process variable (required for Customers/Get?industry=ev to return data)
            var industryIdVar = EventService.GetVariable(ProcessVariableScopeEnum.Process, "industry_id");
            if (!string.IsNullOrWhiteSpace(industryIdVar) && long.TryParse(industryIdVar.Trim(), out var parsedIndustryId))
                entity.IndustryId = parsedIndustryId;

            // 2b. Save / update customer in Supabase biz_customer
            try
            {
                var repo = new SupabaseCustomerRepository();

                // Prefer explicit customer_id from workflow variables (browser/session-bound id)
                var customerIdVar = EventService.GetVariable(ProcessVariableScopeEnum.Process, "customer_id");
                BizCustomerEntity savedEntity = null;

                if (!string.IsNullOrWhiteSpace(customerIdVar))
                {
                    var trimmedId = customerIdVar.Trim();
                    var existing = repo.GetByIdAsync(trimmedId).GetAwaiter().GetResult();
                    if (existing != null)
                    {
                        // Update existing row by customer_id
                        entity.CustomerId = existing.CustomerId;
                        savedEntity = repo.UpdateAsync(existing.CustomerId, entity).GetAwaiter().GetResult() ?? entity;
                        SaveVariableIfSupported("customer_id", existing.CustomerId);
                        System.Diagnostics.Debug.WriteLine($"CustomerContactService: customer updated by id, customer_id={existing.CustomerId}");
                    }
                    else
                    {
                        // No row yet for this id – create new row with this customer_id so it stays stable for this browser/session
                        entity.CustomerId = trimmedId;
                        savedEntity = repo.InsertAsync(entity).GetAwaiter().GetResult();
                        SaveVariableIfSupported("customer_id", savedEntity.CustomerId);
                        System.Diagnostics.Debug.WriteLine($"CustomerContactService: new customer created with provided id, customer_id={savedEntity.CustomerId}");
                    }
                }
                else
                {
                    // Fallback: no explicit customer_id; dedupe by wechat / phone
                    var existing = repo.GetByWechatOrPhoneAsync(entity.Wechat, entity.PhoneNumber ?? entity.Mobile).GetAwaiter().GetResult();
                    if (existing == null)
                    {
                        savedEntity = repo.InsertAsync(entity).GetAwaiter().GetResult();
                        SaveVariableIfSupported("customer_id", savedEntity.CustomerId);
                        System.Diagnostics.Debug.WriteLine($"CustomerContactService: new customer created, customer_id={savedEntity.CustomerId}");
                    }
                    else
                    {
                        entity.CustomerId = existing.CustomerId;
                        savedEntity = repo.UpdateAsync(existing.CustomerId, entity).GetAwaiter().GetResult() ?? existing;
                        SaveVariableIfSupported("customer_id", savedEntity.CustomerId);
                        System.Diagnostics.Debug.WriteLine($"CustomerContactService: customer updated by wechat/phone, customer_id={savedEntity.CustomerId}");
                    }
                }

                // 3. Output BizCustomerEntity JSON to process variable "customer"
                var json = JsonConvert.SerializeObject(savedEntity ?? entity);
                SaveVariableIfSupported("customer", json);
                System.Diagnostics.Debug.WriteLine("CustomerContactService: customer entity written to process variable 'customer'.");

                // 4. Append a structured contact summary into chat_history so that downstream AI nodes
                //    can see which contact fields have already been collected and avoid asking again.
                TryAppendContactSummaryToChatHistory(savedEntity ?? entity);
            }
            catch (Exception ex)
            {
                // Use existing exception logging helper
                var context = contactJson?.Length > 500 ? contactJson.Substring(0, 500) + "..." : contactJson;
                ExceptionLogHelper.RecordException("CustomerContactService", ex, requestData: context);
                throw;
            }
        }

        private BizCustomerEntity BuildBizCustomerFromContact(ContactInfo contact, string existingCustomerId = null)
        {
            var entity = new BizCustomerEntity
            {
                CustomerId = !string.IsNullOrWhiteSpace(existingCustomerId)
                    ? existingCustomerId.Trim()
                    : "cust-" + Guid.NewGuid().ToString("N"),
                Name = contact.Name,
                PhoneNumber = contact.Mobile ?? contact.Telephone,
                Mobile = contact.Mobile,
                Wechat = contact.Wechat,
                Email = contact.Email,
                Stage = "Inquiry"
            };
            return entity;
        }

        /// <summary>
        /// Appends a one-line contact summary message into chat_history as a system-style entry.
        /// This reinforces to the LLM what contact fields have already been collected in this turn.
        /// </summary>
        private void TryAppendContactSummaryToChatHistory(BizCustomerEntity customer)
        {
            if (customer == null || EventService == null)
                return;

            try
            {
                // Build a concise summary including only non-empty fields.
                var parts = new System.Collections.Generic.List<string>();
                if (!string.IsNullOrWhiteSpace(customer.Name))
                    parts.Add($"Name: {customer.Name}");
                if (!string.IsNullOrWhiteSpace(customer.Wechat))
                    parts.Add($"WeChat: {customer.Wechat}");
                if (!string.IsNullOrWhiteSpace(customer.Mobile))
                    parts.Add($"Mobile: {customer.Mobile}");
                if (!string.IsNullOrWhiteSpace(customer.PhoneNumber))
                    parts.Add($"Phone: {customer.PhoneNumber}");
                if (!string.IsNullOrWhiteSpace(customer.Email))
                    parts.Add($"Email: {customer.Email}");

                if (parts.Count == 0)
                    return;

                var summary = "Contact info collected in this turn: " + string.Join("; ", parts);

                var historyJson = EventService.GetVariable(ProcessVariableScopeEnum.Process, "chat_history");
                var historyList = string.IsNullOrWhiteSpace(historyJson)
                    ? new System.Collections.Generic.List<HistoryItem>()
                    : JsonConvert.DeserializeObject<System.Collections.Generic.List<HistoryItem>>(historyJson)
                      ?? new System.Collections.Generic.List<HistoryItem>();

                historyList.Add(new HistoryItem
                {
                    Role = "system",
                    Content = summary
                });

                var newJson = JsonConvert.SerializeObject(historyList);
                EventService.SaveVariable(ProcessVariableScopeEnum.Process, "chat_history", newJson);
                System.Diagnostics.Debug.WriteLine("CustomerContactService: appended contact summary to chat_history.");
            }
            catch (Exception ex)
            {
                // Do not break workflow on history update failure.
                System.Diagnostics.Debug.WriteLine($"CustomerContactService: failed to append contact summary to chat_history: {ex.Message}");
            }
        }

        private static bool HasAnyContactField(ContactInfo contact)
        {
            if (contact == null) return false;
            return !string.IsNullOrWhiteSpace(contact.Name)
                || !string.IsNullOrWhiteSpace(contact.Mobile)
                || !string.IsNullOrWhiteSpace(contact.Telephone)
                || !string.IsNullOrWhiteSpace(contact.Wechat)
                || !string.IsNullOrWhiteSpace(contact.Email);
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

