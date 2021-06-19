using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSeriseUI.Common
{
    [Serializable]
    public class ChartData
    {
        public string IntervalDate { get; set; }
        public double? Value { get; set; }


        public ChartData() : this("", 0)
        {

        }

        public ChartData(string intervalDate, double? value)
        {
            IntervalDate = intervalDate;
            Value = value;
        }
    }
}