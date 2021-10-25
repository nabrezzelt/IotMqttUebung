using System;
using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IotMqttUebung2.Shared
{
    public class MessagePayload
    {
        [JsonPropertyName("ts_meas")]
        public string CurrentDateTime { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("battery")]
        public double Battery { get; set; }

        [JsonPropertyName("load")]
        public double Load { get; set; }

        [JsonIgnore]
        public DateTime ReceivedTimeStamp { get; set; }

        public string AsJsonString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
