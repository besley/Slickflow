using System;

namespace Slickflow.Module.External.Data
{
    /// <summary>
    /// Records plugin exceptions to biz_exception_log in Supabase for troubleshooting.
    /// EventTypeId: 0=Warning, 1=Exception, 2=Error. Priority: 0=Emergency, 1=High, 2=Normal, 3=Low.
    /// </summary>
    public static class ExceptionLogHelper
    {
        private const int EventTypeError = 2;
        private const int PriorityHigh = 1;

        /// <summary>
        /// Records an exception to biz_exception_log (if Supabase is configured). Swallows logging failures.
        /// </summary>
        /// <param name="source">Service or component name, e.g. "CustomerSaveService", "MessageService".</param>
        /// <param name="ex">The exception to log.</param>
        /// <param name="requestData">Optional context (e.g. JSON snippet) to help troubleshooting.</param>
        public static void RecordException(string source, Exception ex, string requestData = null)
        {
            if (ex == null) return;
            try
            {
                var repo = new SupabaseExceptionLogRepository();
                if (!repo.IsConfigured) return;

                var entity = new BizExceptionLogEntity
                {
                    EventTypeId = EventTypeError,
                    Priority = PriorityHigh,
                    Severity = "HIGH",
                    Title = $"{source}: {ex.GetType().Name}",
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerStackTrace = ex.InnerException?.StackTrace,
                    RequestData = requestData,
                    Timestamp = DateTime.UtcNow,
                    Source = source
                };
                repo.InsertAsync(entity).GetAwaiter().GetResult();
            }
            catch
            {
                // Do not let logging failure affect the caller
            }
        }
    }
}
