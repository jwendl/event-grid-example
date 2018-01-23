using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EventHubDataSender.Models
{
    public class Customer
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("birthDate")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("orders")]
        public IEnumerable<Order> Orders { get; set; }
    }
}
