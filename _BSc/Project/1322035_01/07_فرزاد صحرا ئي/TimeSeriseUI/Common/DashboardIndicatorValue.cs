using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSeriseUI.Common
{
    public class DashboardIndicatorValue
    {
        public int IndicatorId { get;  set; }
        public long? IntervalId { get;  set; }
        public string IntervalStrDate { get;  set; }
        public string IndicatorName { get;  set; }
        public decimal? Value { get;  set; }
    }
}