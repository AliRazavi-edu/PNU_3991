using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimeSeriseUI.Common;

namespace TimeSeriseUI.Controls
{
    
    public partial class UCSplineChart : System.Web.UI.UserControl
    {
        private List<ChartViewModel> _vmModel;
        public List<ChartViewModel> vmModel
        {
            get
            {
                
                return _vmModel;
            }
            set
            {
                _vmModel = value;
                var categories = string.Join(",", value.GroupBy(g => g.categoryId).Select(p => p.First().categoryTitle));
                splinecategories.Value = categories;
                var myModel = value.GroupBy(g => g.name).Select(p => new
                {
                    name = p.Key,
                    color = p.First().color,
                    data = p.Select(q => q.value).ToList()
                }).ToList();
                splinedata.Value = JsonConvert.SerializeObject(myModel,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
            }
        }
        private HighChartViewModel _vmHighChartModel;
        public HighChartViewModel vmHighChartModel
        {
            get
            {
                return _vmHighChartModel;
            }
            set
            {
                _vmHighChartModel = value;
                splinecategories.Value = string.Join(",", value.Categories);
                splinedata.Value = JsonConvert.SerializeObject(value.HighChartDataItems,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
            }
        }

        
        public string ChartId { get; set; }


        protected string ChartNewId
        {
            get { return "splinecontainer_" + ChartId; }
        }

        public ChartViewModelList MyChartModel
        {
            set
            {
                vmModel = value.Charts;
                ChartId = value.ChartId;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (vmModel == null || !vmModel.Any())
            {
                if (vmHighChartModel == null)
                {
                    splinecategories.Value = "";
                    splinedata.Value = "";
                }
            }
        }
    }
}