using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSeriseUI.Common
{
    public class ChartViewModel
    {
        public string name { get; set; }
        public string id { get; set; }
        public string color { get; set; }
        public string parent { get; set; }
        public decimal? value { get; set; }
        public long? categoryId { get; set; }
        public string categoryTitle { get; set; }

        public ChartViewModel()
        {

        }
        /// <summary>
        /// For binding indicator data in PieChart and BarChart
        /// </summary>
        /// <param name="values">Indicator Value model which is extracted from CALC</param>
        /// <param name="chartIndicators">Details of the chart Indicator</param>
        public ChartViewModel(DashboardIndicatorValue values, ICollection<ChartIndicator> chartIndicators)
        {
            var chartIndicatorDetail = chartIndicators
                .SingleOrDefault(x => x.IndicatorId == values.IndicatorId);

            color = chartIndicatorDetail.IndicatorColor;
            categoryId = values.IntervalId;
            categoryTitle = values.IntervalStrDate;

            name = $"{values.IndicatorName}";
            value = values.Value;

        }

    }
}