using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSeriseUI.Common
{
    [Serializable]
    public class DataModel
    {
        public int IndicatorId { get; set; }
        public long IndicatorCode { get; set; }

        public string IndicatorTitle { get; set; }

        public DateTime IntervalDate { get; set; }
        public short IntervalId { get; set; }

        public double ActivityValue { get; set; }

        public DataModel(int indicatorId, long indicatorCode, string indicatorTitle, DateTime intervalDate, short intervalId, double activityValue)
        {
            IndicatorId = indicatorId;
            IndicatorCode = indicatorCode;
            IndicatorTitle = indicatorTitle;
            IntervalDate = intervalDate;
            IntervalId = intervalId;
            ActivityValue = activityValue;
        }
    }
}