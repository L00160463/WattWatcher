using System;
using Newtonsoft.Json;

namespace WattWatcher.Data
{

    public class ElectricModel
    {
        [JsonProperty("Amps")]
        public string Amps { get; set; }

        [JsonProperty("ID")]
        public int Id { get; set; }

        [JsonProperty("Kwh")]
        public double Kwh { get; set; }

        [JsonProperty("TimeStamp")]
        public long TimeStamp { get; set; }

        [JsonProperty("Watts")]
        public double Watts { get; set; }
    }
}
