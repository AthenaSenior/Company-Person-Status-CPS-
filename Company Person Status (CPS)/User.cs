using Newtonsoft.Json;

namespace Company_Person_Status__CPS_
{
    public class User
    {
        [JsonProperty(PropertyName = "Id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "AuthorizationLevelId", NullValueHandling = NullValueHandling.Ignore)]
        public int AuthorizationLevelId { get; set; }

        [JsonProperty(PropertyName = "FullName", NullValueHandling = NullValueHandling.Ignore)]
        public string FullName { get; set; }

        [JsonProperty(PropertyName = "Password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "StatusId", NullValueHandling = NullValueHandling.Ignore)]
        public int StatusId { get; set; }

        [JsonProperty(PropertyName = "ThisMonthAwayDuration", NullValueHandling = NullValueHandling.Ignore)]
        public int ThisMonthAwayDuration { get; set; }

        [JsonProperty(PropertyName = "ThisWeekAwayDuration", NullValueHandling = NullValueHandling.Ignore)]
        public int ThisWeekAwayDuration { get; set; }

        [JsonProperty(PropertyName = "TodaysAwayDuration", NullValueHandling = NullValueHandling.Ignore)]
        public int TodaysAwayDuration { get; set; }

        [JsonProperty(PropertyName = "Username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "isDeleted", NullValueHandling = NullValueHandling.Ignore)]
        public bool isDeleted { get; set; }
    }
}
