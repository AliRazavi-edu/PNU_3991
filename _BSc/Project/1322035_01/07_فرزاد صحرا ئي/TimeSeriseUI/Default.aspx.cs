using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimeSeriseUI.Common;
using TimeSeriseUI.Services;
using static TimeSeriseUI.Common.Enums;

namespace WebApplication1
{
    public partial class _Default : Page
    {

        private HighChartViewModel chartModel
        {
            get
            {
                if (ViewState["HighChartViewModel"] != null)
                {
                    return (HighChartViewModel)ViewState["HighChartViewModel"];
                }
                else
                {
                    return new HighChartViewModel(new List<string>());
                }
            }
            set
            {
                ViewState["HighChartViewModel"] = value;
            }
        }

        protected List<double?> ActualActivityTrend
        {
            get { return (List<double?>)ViewState["ActualActivityTrend"]; }
            set { ViewState["ActualActivityTrend"] = value; }
        }

        protected List<string> Categories
        {
            get { return (List<string>)ViewState["Categories"]; }
            set { ViewState["Categories"] = value; }
        }


        protected List<ChartData> TrendComposed
        {
            get { return (List<ChartData>)ViewState["TrendComposed"]; }
            set { ViewState["TrendComposed"] = value; }
        }

        protected List<ChartData> ResidComposed
        {
            get { return (List<ChartData>)ViewState["ResidComposed"]; }
            set { ViewState["ResidComposed"] = value; }
        }

        protected List<ChartData> SeasonalComposed
        {
            get { return (List<ChartData>)ViewState["SeasonalComposed"]; }
            set { ViewState["SeasonalComposed"] = value; }
        }

        protected string IndicatorTitle
        {
            get { return ViewState["MyIndicatorTitle"].ToString(); }
            set { ViewState["MyIndicatorTitle"] = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindControls();
        }

        private void BindControls()
        {
            txtp.Text = "1";
            txtq.Text = "1";
            txtd.Text = "1";
            txts.Text = "12";
            txtPredictCount.Text = "4";
            drpForecatsType.DataSource = Enums.GetForecastsTypeBindable();
            drpForecatsType.DataValueField = "Key";
            drpForecatsType.DataTextField = "Value";
            drpForecatsType.DataBind();
            txtInputvalues.Text = string.Join(",", GetSampleData().Select(x => x.ToString()).ToArray());

        }

        public void FillReport()
        {
            ActualActivityTrend = GetDataFromInput();
            Categories = ActualActivityTrend.Select(x => ActualActivityTrend.IndexOf(x).ToString()).ToList();
            GetTimeSeriseDate(Categories, ActualActivityTrend);
            BindActivityCharts();
        }

        private void BindActivityCharts()
        {
            chartModel = new HighChartViewModel(Categories);
            chartModel.HighChartDataItems.Add(new HighChartDataItems("عملکرد", "#f15c80", ActualActivityTrend));
            chartModel.HighChartDataItems.Add(new HighChartDataItems("روند", "#1a6600", TrendComposed.Select(x => x.Value).ToList()));
            UCSplineChart.ChartId = "activity";
            UCSplineChart.vmHighChartModel = chartModel;

            var chartModelresid = new HighChartViewModel(Categories);
            chartModelresid.HighChartDataItems.Add(new HighChartDataItems("Resid", "#ffff00", ResidComposed.Select(x => x.Value).ToList()));
            chartModelresid.HighChartDataItems.Add(new HighChartDataItems("Seasonal", "#ff4000", SeasonalComposed.Select(x => x.Value).ToList()));
            UCSplineChartResid.ChartId = "Resid";
            UCSplineChartResid.vmHighChartModel = chartModelresid;
        }

        public void GetTimeSeriseDate(List<string> categories, List<double?> actualActivityTrend)
        {
            CultureInfo persianCultureInfo = new CultureInfo("en-US");

            var values = new Dictionary<string, List<string>>
                {
                    {"Date", categories },
                    {"riders", actualActivityTrend.Select(x => x.Value.ToString()).ToList()}
                };


            ForecastModel model = new ForecastModel
            {
                Data = values,
                PredictCount = 0,
                ForcastStep = 0,
                p = 0,
                q = 0,
                d = 0,
                s = 0
            };


            var timeSeriesSrv = TimeSeriesService.Instance();



            var fs = timeSeriesSrv.GetChart<ForecastModel>(model);


            imgPacf.Visible = !string.IsNullOrEmpty(fs);

            imgPacf.ImageUrl = String.Format(@"data:image/jpeg;base64,{0}", fs);



            var serviceResult = timeSeriesSrv.GetTimeSeriesDecomposeResult(model);

            if (!serviceResult.IsSuccessful)
            {
                lbMessage.Text = serviceResult.Message;
                return;
            }

            var composedResult = (TimeSeriesDecomposeModel)serviceResult.Data;


            if (composedResult == null)
            {
                lbMessage.Text = "مقادیر لگهای انتخاب شده متناسب با اعداد دوره های عملکرد نمی باشد!";
                //UcChartBefore.Visible = UcIndicatorView.Visible = false;
                return;
            }

            if (string.IsNullOrEmpty(composedResult.Exception) == false)
            {
                lbMessage.Text =
                "مقادیر لگهای انتخاب شده متناسب با اعداد دوره های عملکرد نمی باشد!" +
                    "<br />" +
                    composedResult.Exception;

                //UcChartBefore.Visible = UcIndicatorView.Visible = false;
                return;
            }




            ResidComposed = new List<ChartData>();

            ResidComposed.AddRange(categories.Select(x => new ChartData(x, null)).ToList());

            for (int i = 0; i < composedResult.Resid.Count; i++)
            {
                ResidComposed[i].Value = composedResult.Resid[i] == null ? new Nullable<double>() : composedResult.Resid[i].Value;


            }

            TrendComposed = new List<ChartData>();



            TrendComposed.AddRange(categories.Select(x => new ChartData(x, null)).ToList());

            for (int i = 0; i < composedResult.Trend.Count; i++)
            {

                TrendComposed[i].Value = composedResult.Trend[i] == null ? new Nullable<double>() : composedResult.Trend[i].Value;


            }



            SeasonalComposed = new List<ChartData>();

            SeasonalComposed.AddRange(categories.Select(x => new ChartData(x, null)).ToList());

            for (int i = 0; i < composedResult.Seasonal.Count; i++)
            {

                SeasonalComposed[i].Value = composedResult.Seasonal[i] == null ? new Nullable<double>() : composedResult.Seasonal[i].Value;


            }



        }

        protected void btnForecast_Click(object sender, EventArgs e)
        {
            var result = ForecastValidation();
            if (!result.IsSuccessful)
            {
                lbMessage.Text = result.Message;
                return;
            }
            else
            {
                BindForecast();
            }
        }

        private void BindForecast()
        {
            //BindGrids();


            int predictCount = Convert.ToInt32(txtPredictCount.Text);
            int p = Convert.ToInt32(txtp.Text);
            int q = Convert.ToInt32(txtq.Text);
            int s = Convert.ToInt32(txts.Text);
            int d = Convert.ToInt32(txtd.Text);
            int forcastStep = 4;

            var forecastType = ((Enums.ForecastsType)Convert.ToInt32(drpForecatsType.SelectedValue)).ToString();

            CultureInfo persianCultureInfo = new CultureInfo("en-US");
            var values = new Dictionary<string, List<string>>
                {
                    {"Date", Categories},
                    {"riders", ActualActivityTrend.Select(x => x.Value.ToString()).ToList()}
                };


            ForecastModel model = new ForecastModel
            {
                Data = values,
                PredictCount = predictCount,
                ForcastStep = forcastStep,
                p = p,
                q = q,
                d = d,
                s = s,
                type = forecastType
            };


            var timeSeriseSrv = TimeSeriesService.Instance();

            var serviceResult = timeSeriseSrv.GetTimeSeriesForcastResult(model);

            if (!serviceResult.IsSuccessful)
            {
                lbMessage.Text = serviceResult.Message;
                return;
            }

            var prgressResult = (TimeSeriDto)serviceResult.Data;


            if (string.IsNullOrEmpty(prgressResult.Exception) == false)
            {
                lbMessage.Text =
                    "مقادیر لگهای انتخاب شده متناسب با اعداد دوره های عملکرد نمی باشد!" +
                    "<br />" +
                    prgressResult.Exception;
                return;
            }
            if (prgressResult.Forecast is null || prgressResult.Forecast.Count == 0)
            {
                lbMessage.Text = "با مقادیر ارسال شده ، سیستم قادر به انجام عملیات پیش بینی نمی باشد!";
                return;
            }


            var forecastResult = prgressResult.Forecast;

            var predictResult = prgressResult.PredictResult;

            //ChartModel cmActivity = new ChartModel() { ChartType = ChartTypes.Line, Data = chartData };

            List<ChartData> cmPrePredicat = predictResult.Select(x => new ChartData()
            {
                IntervalDate = x.Key.ToString(),
                Value = x.Value
            }).ToList();


            List<ChartData> cdPredicate = new List<ChartData>();
            foreach (var item in Categories)
            {
                double? val = null;
                if (cmPrePredicat.Any(x => x.IntervalDate == item))
                    val = cmPrePredicat.Single(x => x.IntervalDate == item).Value;
                cdPredicate.Add(new ChartData(item, val));
            }

            //ChartModel cmPredicat = new ChartModel() { ChartType = ChartTypes.Line, Data = cdPredicate };

            List<ChartData> cdForcast = new List<ChartData>();






            var joinerPoint = Categories.Last();
            var joinerTrendPoint = ActualActivityTrend.Last();
            cdForcast.AddRange(Categories.Select(x => new ChartData(x, null)).ToList());

            var checkExists = cdForcast.SingleOrDefault(x => x.IntervalDate == joinerPoint);

            if (checkExists != null)
            {
                cdForcast.Remove(checkExists);
            }

            cdForcast.Add(new ChartData(joinerPoint, joinerTrendPoint));


            for (int i = 0; i < forcastStep; i++)
            {
                int forcastIndex = Convert.ToInt32(joinerPoint) + i + 1;
                cdForcast.Add(new ChartData(forcastIndex.ToString(), forecastResult.ElementAt(i).Value));
            }
            //UcIndicatorView.Visible = true;
            //ChartModel cmForecast = new ChartModel() { ChartType = ChartTypes.Line, Data = cdForcast };


            var chartModelforcast = new HighChartViewModel(Categories);
            chartModelforcast.HighChartDataItems.Add(new HighChartDataItems("عملکرد", "#2962ff", ActualActivityTrend));
            chartModelforcast.HighChartDataItems.Add(new HighChartDataItems("تطبیق", "#d7ccc8", cdPredicate.Select(x => x.Value).ToList()));
            chartModelforcast.HighChartDataItems.Add(new HighChartDataItems("پیش بینی", "#ab47bc", cdForcast.Select(x => x.Value).ToList()));
            UCSplineForcast.ChartId = "chartModelforcast";
            UCSplineForcast.vmHighChartModel = chartModelforcast;

            BindActivityCharts();






            //var forecastsData =
            //    (from x in cdForcast

            //     let activity = AiFChartData.SingleOrDefault(y => y.IntervalDate == x.IntervalDate)

            //     where
            //     x.IntervalDate != joinerPoint.IntervalDate && x.Value.HasValue

            //     orderby x.IntervalDate
            //     let percent = (activity?.Value.HasValue == true) ? ((x.Value - activity.Value.Value) / ((activity.Value.Value + x.Value) / 2)) * 100 : 0
            //     select new
            //     {
            //         IntervalDate = x.IntervalDate,
            //         Value = x.Value,
            //         ActivityValue = activity?.Value,
            //         Diff =
            //         activity?.Value.HasValue == true ?
            //         percent.Value.ToString("N2") : "",



            //         Increase = (activity?.Value.HasValue == true) &&

            //            ((x.Value < activity.Value.Value && Math.Abs(percent.Value) <= 5) || (x.Value >= activity.Value.Value))

            //            ? true : false,

            //         Decrease = (activity?.Value.HasValue == true) &&

            //                (x.Value < activity.Value.Value && (Math.Abs(percent.Value)) > 5) ? true : false

            //     }
            //     ).ToList();

            ////cdForcast.Where(x => x.IntervalDate != joinerPoint.IntervalDate && x.Value.HasValue).OrderBy(x => x.IntervalDate).ToList();

            //var statisticDataSource = new List<TimeSeriDto>();
            //statisticDataSource.Add(new TimeSeriDto
            //{
            //    aic = prgressResult.aic,
            //    hqic = prgressResult.hqic,
            //    Exception = string.Empty,
            //    Forecast = null,
            //    PredictResult = null
            //});
            //lvStatistics.DataSource = statisticDataSource;
            //lvStatistics.DataBind();
            //lvStatistics.Visible = true;

            //lvForecast.DataSource = forecastsData;
            //lvForecast.DataBind();
            //lvForecast.Visible = true;


            //UcChartBefore.SetChart(IndicatorTitle, chartModelFirst, new Dictionary<string, string>() { { "جزئیات", "$('.tables2').html($('#pnl3').html()); $('#mainContainer2').dialog('open');" } });
            //UcIndicatorView.SetChart(IndicatorTitle, chartModelSecond, new Dictionary<string, string>() { { "جزئیات", "$('.tables1').html($('#pnl2').html()); $('#mainContainer').dialog('open');" } });

        }

        private ServiceResultSet ForecastValidation()
        {

            //SARIMAX d & D Distance   AND  p & P PartialAutoCorlation   AND q & Q AutoCorrolation  AND s Seasonality
            int d = Convert.ToInt32(txtd.Text);
            int D = 1;
            int s = Convert.ToInt32(txts.Text);
            int p = Convert.ToInt32(txtp.Text);
            int P = 1;
            int q = Convert.ToInt32(txtq.Text);
            int Q = 0;

            var maximumActictyCount = d + D * s + GetMaxOfArray(3 * q + 1, 3 * Q * s + 1, p, P * s) + 1;




            return ServiceResultSet.GetSuccessful("", null);
        }
        private int GetMaxOfArray(params int[] intItems)
        {
            return intItems.Max(x => x);
        }

        protected void drpForecatsType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedValue = drpForecatsType.SelectedValue;

            var selectedType = (ForecastsType)int.Parse(selectedValue);


            switch (selectedType)
            {
                case ForecastsType.sarima:
                    txts.Visible = true;
                    txtd.Visible = true;
                    txtq.Visible = true;
                    txtp.Visible = true;

                    break;
                case ForecastsType.arima:

                    txts.Visible = false;

                    txtd.Visible = true;
                    txtq.Visible = true;
                    txtp.Visible = true;

                    break;
                case ForecastsType.arma:

                    txts.Visible = false;
                    txtd.Visible = false;

                    txtq.Visible = true;
                    txtp.Visible = true;

                    break;
                //case ForecastsType.Var:
                //    txts.Visible = true;
                //    txtd.Visible = true;
                //    txtq.Visible = true;
                //    txtp.Visible = true;
                //    break;
                //case ForecastsType.ArtificialNeuralNetwork:
                //    txts.Visible = true;
                //    txtd.Visible = true;
                //    txtq.Visible = true;
                //    txtp.Visible = true;
                //    break;
                default:
                    break;
            }





        }

        private List<double?> GetSampleData()
        {
            return new List<double?>() { 180946564945902, 183290289067017, 183783262691986, 181680213984661, 185280227592116, 183934680609557, 185601489965695, 186386911688507, 188520904995968, 189779314764548, 187071783668207, 184672224089606, 186120108294893, 187617165731862, 188756832742596, 191508972342026, 219650070119769, 202313574029626, 233382677022496, 246981163154860, 261177341149808, 227981934199572, 253437763121271, 227730584373886, 226723386894179, 264051805671939, 231665253182757, 231431316387964, 236736543917396, 269329650654244, 266975673501245, 271980257852669, 270096026618072, 269855949479145, 262804588321090, 267551363395431, 342477455716015, 340381813002599, 336025262735346, 335187607053901, 331049964255165, 337330332865996, 351612551450915, 351714396047640, 362309190130634, 366220974781093, 331020492460755, 370626530526490, 375634578890903, 499728677145879, 373392276183452, 447644350657593, 436716474862850, 431810165775519, 421612429025162, 390985109678939, 394880993666669, 374394531457468, 372149145558837, 382589360166926, 397549713760905, 407890215018630, 428969314175893, 432138444592839, 544772752192068, 572277270779974, 576804442156361, 591252604111424, 594389830264410, 599235661367756, 619335578178728 };
        }

        private List<double?> GetDataFromInput()
        {
            var values = txtInputvalues.Text.Split(Convert.ToChar(","));
            return values.Select(x => (double?)Convert.ToDouble(x)).ToList();
        }

        protected void btnSubmitTrend_Click(object sender, EventArgs e)
        {
            FillReport();
        }
    }
}