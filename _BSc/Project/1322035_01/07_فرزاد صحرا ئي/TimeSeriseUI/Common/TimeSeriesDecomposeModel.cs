using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSeriseUI.Common
{
    public class TimeSeriesDecomposeModel
    {
        public string Exception { get; set; }


        public Dictionary<int, double?> Resid { get; set; }


        public Dictionary<int, double?> Seasonal { get; set; }


        public Dictionary<int, double?> Trend { get; set; }


        public TimeSeriesDecomposeModel()
        {

        }
    }
}