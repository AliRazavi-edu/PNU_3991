using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSeriseUI.Common
{
    public class ChartViewModelList
    {
        public string ChartId { get; set; }
        public string ChartTitle { get; set; }
        public List<ChartViewModel> Charts { get; set; }
    }
}