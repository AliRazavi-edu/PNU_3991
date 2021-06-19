using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSeriseUI.Common
{
    [Serializable]
    public class HighChartDataItems
    {
        public HighChartDataItems(string name, string color, List<double?> data)
        {
            this.Name = name;
            this.Color = color;
            this.Data = data;
        }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }
        [JsonProperty(PropertyName = "data")]
        public List<double?> Data { get; set; }
    }
}