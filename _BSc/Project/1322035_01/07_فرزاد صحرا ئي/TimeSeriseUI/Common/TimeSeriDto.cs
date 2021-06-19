using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSeriseUI.Common
{
    public class TimeSeriDto
    {
        public Dictionary<string, double> PredictResult { get; set; }
        public Dictionary<string, double> Forecast { get; set; }
        public float aic { get; set; }
        public float hqic { get; set; }
        public string Exception { get; set; }
    }
}