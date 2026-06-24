using System;
using Newtonsoft.Json;

namespace Slickflow.Module.External.Customer.Entity
{
    /// <summary>
    /// Entity for Supabase table biz_customer.
    /// Properties map to DB columns (snake_case serialization for Supabase).
    /// </summary>
    public class BizCustomerEntity
    {
        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("wechat")]
        public string Wechat { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("stage")]
        public string Stage { get; set; }

        [JsonProperty("industry_id")]
        public long? IndustryId { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
