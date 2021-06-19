using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSeriseUI.Common
{
    [Serializable]
    public class HighChartViewModel
    {
        public HighChartViewModel(List<string> categories)
        {
            Categories = categories;
            HighChartDataItems = new List<HighChartDataItems>();
        }

        public List<string> Categories { get; set; }

        public List<HighChartDataItems> HighChartDataItems { get; set; }

    }
}