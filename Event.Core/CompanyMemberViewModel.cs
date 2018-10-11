namespace Event.Core
{
    using System;

    using Newtonsoft.Json;

    public class CompanyMemberViewModel
    {
        [JsonProperty("companyId")]
        public long CompanyId { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("userId")]
        public long UserId { get; set; }
    }

    public class CompanyMemberDisplayViewModel
    {
        public DateTime CreatedDate { get; set; }

        public string Firstname { get; set; }

        public long Id { get; set; }

        public string Lastname { get; set; }

        public string Name { get; set; }

        public string UserEmail { get; set; }
    }
}