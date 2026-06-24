using System;
using Slickflow.Engine.Common;
using Slickflow.Engine.External;
using Slickflow.Module.External.Customer.Entity;
using Slickflow.Module.External.Data;

namespace Slickflow.Module.External.Customer
{
    /// <summary>
    /// Saves customer–AI chat to Supabase biz_conversation from process variables:
    /// user_message, ai_response, and customer (or customer_id) for association.
    /// </summary>
    public class ConversationService : ExternalServiceBase, IExternalService
    {
        public override void Execute()
        {
            if (EventService == null)
            {
                System.Diagnostics.Debug.WriteLine("MessageService: EventService is null, skip.");
                return;
            }

            var processInstanceId = EventService.GetProcessInstanceId();
            var userMessage = EventService.GetVariable(ProcessVariableScopeEnum.Process, "user_message");
            var aiResponse = EventService.GetVariable(ProcessVariableScopeEnum.Process, "ai_response");
            var customerId = ResolveCustomerId();
            var sessionId = EventService.GetVariable(ProcessVariableScopeEnum.Process, "session_id");
            var industryIdVar = EventService.GetVariable(ProcessVariableScopeEnum.Process, "industry_id");
            long? industryId = null;
            if (!string.IsNullOrWhiteSpace(industryIdVar) && long.TryParse(industryIdVar.Trim(), out var parsedId))
                industryId = parsedId;

            // Require at least one of user_message or ai_response to save a record
            var hasContent = !string.IsNullOrWhiteSpace(userMessage) || !string.IsNullOrWhiteSpace(aiResponse);
            if (!hasContent)
            {
                System.Diagnostics.Debug.WriteLine("MessageService: user_message and ai_response both empty, skip.");
                return;
            }

            var entity = new BizConversationEntity
            {
                ProcessInstanceId = processInstanceId.ToString(),
                CustomerId = customerId,
                SessionId = string.IsNullOrWhiteSpace(sessionId) ? null : sessionId.Trim(),
                UserMessage = userMessage ?? "",
                AiResponse = aiResponse ?? "",
                IndustryId = industryId
            };

            try
            {
                var repo = new SupabaseConversationRepository();
                repo.InsertAsync(entity).GetAwaiter().GetResult();
                System.Diagnostics.Debug.WriteLine($"MessageService: conversation saved, conversation_id={entity.ConversationId}");
            }
            catch (Exception ex)
            {
                var context = $"ProcessInstanceId={processInstanceId}, CustomerId={customerId ?? "(null)"}";
                ExceptionLogHelper.RecordException("MessageService", ex, requestData: context);
                throw;
            }
        }

        /// <summary>
        /// Resolves customer_id from process variables: prefer "customer_id", then parse "customer" as JSON or plain id.
        /// </summary>
        private string ResolveCustomerId()
        {
            var customerId = EventService.GetVariable(ProcessVariableScopeEnum.Process, "customer_id");
            if (!string.IsNullOrWhiteSpace(customerId))
                return customerId.Trim();

            var customer = EventService.GetVariable(ProcessVariableScopeEnum.Process, "customer");
            if (string.IsNullOrWhiteSpace(customer))
                return null;

            customer = customer.Trim();
            // If it looks like a customer_id (e.g. cust-xxx), use as-is
            if (customer.StartsWith("cust-", StringComparison.OrdinalIgnoreCase))
                return customer;
            // Try parse as JSON for customer_id
            try
            {
                var obj = Newtonsoft.Json.Linq.JObject.Parse(customer);
                var id = obj["customer_id"] ?? obj["CustomerId"];
                return id?.ToString();
            }
            catch
            {
                return null;
            }
        }
    }
}
