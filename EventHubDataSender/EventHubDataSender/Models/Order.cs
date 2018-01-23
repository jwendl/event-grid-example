using Newtonsoft.Json;

namespace EventHubDataSender.Models
{
    public class Order
    {
        [JsonProperty("item")]
        public string Item { get; set; }

        [JsonProperty("serialNumber")]
        public string SerialNumber { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }
    }
}
