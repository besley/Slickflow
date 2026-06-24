using System;
using Newtonsoft.Json;
using Slickflow.Engine.Common;
using Slickflow.Engine.External;
using Slickflow.Module.External.Customer.Entity;
using Slickflow.Module.External.Utility;

namespace Slickflow.Module.External.Customer
{
    /// <summary>
    /// Extracts customer contact from process variable user_message and writes the Customer entity to process variable "customer" (JSON).
    /// Does not call Supabase; use CustomerSaveService afterwards to persist to biz_customer.
    /// </summary>
    public class ContactExtractService : ExternalServiceBase, IExternalService
    {
        public override void Execute()
        {
            if (EventService == null)
            {
                System.Diagnostics.Debug.WriteLine("CustomerExtractService: EventService is null, skip.");
                return;
            }

            var userMessage = EventService.GetVariable(ProcessVariableScopeEnum.Process, "user_message");
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                System.Diagnostics.Debug.WriteLine("CustomerExtractService: user_message is empty, skip.");
                return;
            }

            var contact = ContactExtractorUtility.ExtractAll(userMessage);
            var hasAny = !string.IsNullOrWhiteSpace(contact.Name) || !string.IsNullOrWhiteSpace(contact.Mobile)
                || !string.IsNullOrWhiteSpace(contact.Telephone) || !string.IsNullOrWhiteSpace(contact.Wechat)
                || !string.IsNullOrWhiteSpace(contact.Email);
            if (!hasAny)
            {
                System.Diagnostics.Debug.WriteLine("CustomerExtractService: no contact info extracted from user_message.");
                return;
            }

            // Prefer existing customer_id from workflow variables (e.g. browser session-bound id),
            // so all multi-turn messages from the same browser map to the same biz_customer row.
            var existingCustomerId = EventService.GetVariable(ProcessVariableScopeEnum.Process, "customer_id");
            var entity = BuildBizCustomerFromContact(contact, existingCustomerId);
            var json = JsonConvert.SerializeObject(entity);
            SaveVariableIfSupported("customer", json);
            System.Diagnostics.Debug.WriteLine("CustomerExtractService: customer entity written to process variable 'customer'.");
        }

        internal static BizCustomerEntity BuildBizCustomerFromContact(ContactInfo contact, string existingCustomerId = null)
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
