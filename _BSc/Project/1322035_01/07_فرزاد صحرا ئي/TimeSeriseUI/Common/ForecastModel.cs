using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSeriseUI.Common
{
    [Serializable]
    public class ForecastModel
    {
        public int PredictCount { get; set; }
        public int ForcastStep { get; set; }
        public Dictionary<string, List<string>> Data { get; set; }

        public int p { get; set; }
        public int q { get; set; }

        public int d { get; set; }

        public int s { get; set; }


        public string type { get; set; }
    }
}