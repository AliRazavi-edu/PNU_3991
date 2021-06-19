using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using TimeSeriseUI.Common;

namespace TimeSeriseUI.Services
{
    public class TimeSeriesService
    {
        public static TimeSeriesService Instance()
        {
            return new TimeSeriesService();
        }

        public static async System.Threading.Tasks.Task<Y> ExportToService<T, Y>(T inputData, string url)
        {
            try
            {
                string serviceAnswer = "";
                var JsonData = JsonConvert.SerializeObject(inputData);


                //var JsonData = "{\"Exception\":\"\",\"PredictResult\":{\"2012-06-20\":104213092.88135915,\"2012-07-21\":106959065.77932782,\"2012-08-21\":105658488.67159687,\"2012-09-21\":103946582.58526957,\"2012-10-21\":111543922.23739035,\"2012-11-20\":115920841.57162552,\"2012-12-20\":122045677.91306259,\"2013-01-19\":135661043.8753435,\"2013-02-18\":136509893.65268418,\"2013-03-20\":151281093.3643688,\"2013-04-20\":135586031.0031166,\"2013-05-21\":202889670.34357607,\"2013-06-21\":151511579.00474775,\"2013-07-22\":144259831.09890848,\"2013-08-22\":141242964.28931466,\"2013-09-22\":146467327.10282895,\"2013-10-22\":151707709.42769963,\"2013-11-21\":148641051.02718353,\"2013-12-21\":150169843.61874527,\"2014-01-20\":140200277.12822998,\"2014-02-19\":154459097.063882,\"2014-03-20\":133206220.26760657,\"2014-04-20\":153029629.84165904,\"2014-05-21\":176370777.42225558,\"2014-06-21\":150776574.4068814,\"2014-07-22\":147127481.76820174,\"2014-08-22\":142540552.10690063,\"2014-09-22\":152456479.88337526,\"2014-10-22\":152747167.8771231,\"2014-11-21\":143303451.21930635,\"2014-12-21\":155149146.0932965,\"2015-01-20\":148249515.40233225,\"2015-02-19\":146879311.05148092,\"2015-03-20\":142211508.78417045,\"2015-04-20\":142041911.6043919,\"2015-05-21\":132034732.72347996,\"2015-06-21\":127286239.01554383,\"2015-07-22\":128623130.85799833,\"2015-08-22\":123101223.05681501,\"2015-09-22\":135503150.54076475,\"2015-10-22\":129836957.58790402,\"2015-11-21\":129770696.42036857,\"2015-12-21\":127488495.5010468,\"2016-01-20\":125407317.96432686,\"2016-02-19\":151013623.68472934,\"2016-03-19\":131900530.41142562,\"2016-04-19\":127332082.8389213,\"2016-05-20\":124563352.77025495,\"2016-06-20\":132011326.58557424,\"2016-07-21\":154271467.36924046,\"2016-08-21\":164471045.22383034,\"2016-09-21\":187287408.68643847,\"2016-10-21\":178279404.77023774,\"2016-11-20\":188709626.60797513,\"2016-12-20\":186843962.93011636,\"2017-01-19\":205821202.0942423,\"2017-02-18\":173690758.72409678,\"2017-03-20\":181420742.9475616,\"2017-04-20\":172607807.0378668,\"2017-05-21\":167826910.9722534,\"2017-06-21\":199940683.24807617,\"2017-07-22\":193016363.3891886,\"2017-08-22\":181096366.707524,\"2017-09-22\":200541649.59927005,\"2017-10-22\":192513644.78545284,\"2017-11-21\":193659196.1027687,\"2017-12-21\":204116737.4673779,\"2018-01-20\":212390461.8934974,\"2018-02-19\":230770913.09120017,\"2018-03-20\":223437765.82971534,\"2018-04-20\":205876217.94103485,\"2018-05-21\":212031059.99970335,\"2018-06-21\":253758708.958096,\"2018-07-22\":250616193.4888049,\"2018-08-22\":257631457.04407048,\"2018-09-22\":316832935.7101959,\"2018-10-22\":302211226.990698,\"2018-11-21\":322518391.2613634,\"2018-12-21\":333043545.408749,\"2019-01-20\":345704247.41142493,\"2019-02-19\":354588594.3888728,\"2019-03-20\":328442693.56075406,\"2019-04-20\":330154072.0510785,\"2019-05-21\":359705962.9996399,\"2019-06-21\":493284839.6000559,\"2019-07-22\":466061839.3753756,\"2019-08-22\":485443623.81863993,\"2019-09-22\":515959533.9606901,\"2019-10-22\":519564782.64130074,\"2019-11-21\":477546871.0126082,\"2019-12-21\":557464430.8251523,\"2020-01-20\":600655992.3363004},\"aic\":0.0,\"forecast\":{\"93\":633192450.0408834,\"94\":591192506.6783643},\"hqic\":0.0}";


                var httpContent = new StringContent(JsonData, Encoding.UTF8, "application/json");
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                //File.WriteAllText(@"D:\WebSites\budget.cbs.branch.bm\Files\Json" + Guid.NewGuid().ToString() + ".txt", url + Environment.NewLine + JsonData);

                // local file logging
                //File.WriteAllText(@"D:\DevelopmentOnline\BankMellat.BudgetProject\Files\Json" + Guid.NewGuid().ToString() + ".txt", url + Environment.NewLine + JsonData);
                using (var httpClient = new HttpClient())
                {
                    var httpResponse = await httpClient.PostAsync(url, httpContent).ConfigureAwait(false);
                    if (httpResponse.Content != null)
                    {
                        serviceAnswer = await httpResponse.Content.ReadAsStringAsync();

                        serviceAnswer = serviceAnswer.Replace("\"null\"", "null");
                    }
                }


                //File.WriteAllText(@"D:\WebSites\budget.cbs.branch.bm\Files\Result" + Guid.NewGuid().ToString() + ".txt", serviceAnswer);

                // local file logging
                //File.WriteAllText(@"D:\DevelopmentOnline\BankMellat.BudgetProject\Files\Result" + Guid.NewGuid().ToString() + ".txt", serviceAnswer);

                ////if (serviceAnswer.Contains("500 Internal Server Error")) return JsonConvert.DeserializeObject<Y>(string.Empty);

                //// Regression Sample Data For Test Offline
                //serviceAnswer = "{\"aic\":270.08300954569984,\"bic\":269.86665014192107,\"coef\":[7.105753312536727,0.4218804903784825,0.224982171504978,472304586.939518],\"pvalues\":[0.12810418510790683,0.8744017692142818,0.899628047262121,0.7796178027486571],\"stdError\":[3.404248765433658,2.4531089234043675,1.6408818758584545,1543530729.3234231],\"t\":[2.0873190539680033,0.17197788746901893,0.13711052258851653,0.30598975321116123]}";

                //// Neural Network Smaple Data for Test Offline
                //serviceAnswer = "{\"actualActivityTrend\":[171759944.55,161204493.32,166380556.38,168860539.65,171567213.82,167420568.87,172248418.51,169631299.03,166151321.81,168495677.85,168909416.67,169395576.39,164639290.87,183283478.56,180177808.72,175877324.97,172046810.32,169480576.64,170366087.97,169164618.16,168865857.68,164520970.06,171793737.02,171593028.75,175515703.57,177906100.97,204311619.14,181944756.58,185975804.11,188935535.83,180304753.96,175456852.9,179415179.56,177028106.69,177160411.0,188709294.11,193948243.7,199955541.08,196886429.33,219782839.43,196665466.63,210566290.09,220528281.37,215665358.67,220764303.04,227090919.11,235121249.84,215628719.56,238003820.99,250243521.83,238499251.51,226755258.83,205048444.41,206364433.43,209870210.8,218359689.82,213888006.87,206973048.22,209589358.41,217495349.48,219681566.18,226077377.19,221062869.72,231530013.58,243354768.27,269762476.9,243505250.96,245765931.65,244964205.77,248641645.87,254189888.55,261393662.66,256391704.99,265693565.63,290397217.49,288670653.95,300759153.77,305250766.56,317640622.03,370440825.41,313230128.58,315605957.24,305379844.98,307760245.79,300567519.57,303828779.96,306766830.34,316967713.12,317592326.89,310502462.73,305391535.14,342353090.89,332814031.86,330836559.9,334697328.69,330751821.19,342143634.15,349530997.44,360746724.31,358128681.85,369214318.45,377628909.07,356472764.84,367459789.52,331604630.61,346429547.4,347984638.99,349161418.06,354636570.32,369358119.96,387189506.77,394268584.82,446441802.89,460169622.26,455629762.68,461378658.76,466202951.37,476596304.83,440078099.61,464512850.94,439191361.53,437707450.99,446250002.38,436872111.24,441955272.11,446174722.54,502375799.84,491563469.64,492638308.26,483759584.96,506049412.36,486633261.63,480106114.65,474007904.1,0.0,0.0,0.0,0.0,0.0],\"evaluationTrend\":[483759584.96,506049412.36,486633261.63,480106114.65,474007904.1,0.0,0.0,0.0,0.0,0.0],\"mseEvaluation\":\"0.9926443\",\"mseTest\":\"0.004879082\",\"mseTrain\":\"0.30605572\",\"predictTrend\":[310740736.0,506049408.0,437455744.0,349755648.0,227005184.0,238580224.0,146832640.0,72656384.0,0.0],\"testTrend\":[373647721.2100816,506049412.36000013,40841320.71795988,352117408.0145936,29545099.35390377,10641454.272715092,119465642.26209307,0.0],\"trainTrend\":[8607984.0,9174064.0,13473872.0,12132688.0,3645168.0,5287568.0,5155696.0,7345424.0,1180336.0,28315600.0,40318528.0,35838480.0,28770736.0,12981872.0,10167280.0,8345008.0,7456720.0,0.0,7584112.0,13283648.0,21949776.0,32825952.0,76927584.0,69941600.0,64682416.0,69700032.0,48527248.0,38628112.0,36788448.0,31952208.0,32562272.0,54106096.0,73441792.0,92609728.0,99203376.0,132869736.0,116652560.0,127710336.0,148830112.0,151088320.0,162891680.0,176110752.0,192070976.0,172200992.0,196249008.0,220576608.0,214596592.0,194445520.0,150229472.0,129418704.0,127312016.0,145456352.0,149380160.0,138479808.0,135633792.0,147193632.0,157326240.0,172922176.0,172282400.0,186478096.0,208880800.0,250485040.0,237337952.0,248518960.0,252098144.0,262922992.0,262671328.0,287959872.0,292679072.0,318876032.0,370523200.0,397447584.0,427032768.0,449641152.0,484015680.0,506049408.0,471849920.0,448784608.0,431155872.0,420545408.0,452327232.0,455106400.0,463626880.0,472002432.0,475741760.0,465512832.0,455251776.0,476941888.0,476743040.0,472750400.0,463903232.0,448637312.0,460538944.0,466484512.0,471574912.0,465542848.0,462910304.0,462349504.0,440152832.0,443151936.0,415041472.0,425593856.0,442264704.0,447238720.0,461018368.0,464866560.0]}";
                return JsonConvert.DeserializeObject<Y>(serviceAnswer);
            }
            catch (Exception ex)
            {
                return default(Y);
            }
        }


        //public string GetChart<T>(T inputData, string url)
        //{
        //    string base64String = string.Empty;



        //    string fileRootPath = Extentions.GetPythonModulesPath();

        //    var fileName = ExportToService<T, ACFResultModel>(inputData, url);

        //    fileName.Wait();

        //    string filePath = Path.Combine(fileRootPath, "PacfPlot", fileName.Result.FileName);


        //    using (Image image = Image.FromFile(filePath))
        //    {
        //        using (MemoryStream m = new MemoryStream())
        //        {
        //            image.Save(m, image.RawFormat);
        //            byte[] imageBytes = m.ToArray();
        //            base64String = Convert.ToBase64String(imageBytes);
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(base64String))
        //        File.Delete(filePath);


        //    return base64String;
        //}
        public  string GetPythonModulesPath()
        {
            return ConfigurationManager.AppSettings["PythonModulesPath"].ToString();
        }

        public string GetChart<T>(T inputData)
        {
            string base64String = string.Empty;


            string fileRootPath = GetPythonModulesPath();

            var fileName = ExportToService<T, ACFResultModel>(inputData, GetUrl(Enums.PredictionModelFuncTypes.PartialAutocorrelation));

            fileName.GetAwaiter().GetResult();

            string filePath = Path.Combine(fileRootPath, "PacfPlot", fileName.Result.FileName);

            Bitmap btmS = new Bitmap(filePath);

            Bitmap btmT = Crop(btmS);

            int height = btmT.Height;

            string newPath = Path.GetDirectoryName(filePath) + "\\" + Path.GetRandomFileName();

            btmT.Save(newPath);
            btmS.Dispose();
            btmT.Dispose();


            using (Image image = Image.FromFile(newPath))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);
                }
            }
            if (!string.IsNullOrEmpty(base64String))
            {
                File.Delete(filePath);
                File.Delete(newPath);
            }

            return base64String;
        }

        public static Bitmap Crop(Bitmap bmp)
        {
            int w = bmp.Width;
            int h = bmp.Height;

            Func<int, bool> allWhiteRow = row =>
            {
                for (int i = 0; i < w; ++i)
                    if (bmp.GetPixel(i, row).R != 255)
                        return false;
                return true;
            };

            Func<int, bool> allWhiteColumn = col =>
            {
                for (int i = 0; i < h; ++i)
                    if (bmp.GetPixel(col, i).R != 255)
                        return false;
                return true;
            };

            int topmost = 0;
            for (int row = 0; row < h; ++row)
            {
                if (allWhiteRow(row))
                    topmost = row;
                else break;
            }

            int bottommost = 0;
            for (int row = h - 1; row >= 0; --row)
            {
                if (allWhiteRow(row))
                    bottommost = row;
                else break;
            }

            int leftmost = 0, rightmost = 0;
            for (int col = 0; col < w; ++col)
            {
                if (allWhiteColumn(col))
                    leftmost = col;
                else
                    break;
            }

            for (int col = w - 1; col >= 0; --col)
            {
                if (allWhiteColumn(col))
                    rightmost = col;
                else
                    break;
            }

            if (rightmost == 0) rightmost = w; // As reached left
            if (bottommost == 0) bottommost = h; // As reached top.

            int croppedWidth = rightmost - leftmost;
            int croppedHeight = bottommost - topmost;

            if (croppedWidth == 0) // No border on left or right
            {
                leftmost = 0;
                croppedWidth = w;
            }

            if (croppedHeight == 0) // No border on top or bottom
            {
                topmost = 0;
                croppedHeight = h;
            }

            try
            {
                var target = new Bitmap(croppedWidth, croppedHeight);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(bmp,
                      new RectangleF(0, 0, croppedWidth, croppedHeight),
                      new RectangleF(leftmost, topmost, croppedWidth, croppedHeight),
                      GraphicsUnit.Pixel);
                }
                return target;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public ARIMATrendModel GetARIMATrendModel<T>(T inputData, string url)
        //{
        //    var at = ExportToService<T, ARIMATrendModel>(inputData, url);
        //    return (ARIMATrendModel)at.Result;
        //}

        
        
        
        public ServiceResultSet GetTimeSeriesDecomposeResult(ForecastModel model)
        {
            var composed = ExportToService<ForecastModel, TimeSeriesDecomposeModel>(model, GetUrl(Enums.PredictionModelFuncTypes.Decomposition));
            composed.GetAwaiter().GetResult();
            if (composed == null)
            {
                return ServiceResultSet.GetUnSuccessful("با این شرایط امکان تخمین وجود ندارد!");
            }
            TimeSeriesDecomposeModel composedResult = composed.Result;
            if (composedResult == null)
            {
                return ServiceResultSet.GetUnSuccessful("با این شرایط امکان تخمین وجود ندارد!");
            }
            return ServiceResultSet.GetSuccessful("done!", composedResult);
        }
        public ServiceResultSet GetTimeSeriesForcastResult(ForecastModel model)
        {
            var composed = ExportToService<ForecastModel, TimeSeriDto>(model, GetUrl(Enums.PredictionModelFuncTypes.Forecast));
            composed.Wait();
            if (composed == null)
            {
                return ServiceResultSet.GetUnSuccessful("با این شرایط امکان تخمین وجود ندارد!");
            }
            TimeSeriDto composedResult = composed.Result;
            if (composedResult == null)
            {
                return ServiceResultSet.GetUnSuccessful("با این شرایط امکان تخمین وجود ندارد!");
            }
            return ServiceResultSet.GetSuccessful("done!", composedResult);
        }
        public string GetUrl(Enums.PredictionModelFuncTypes predictionModelFuncType)
        {
            var rootPath = ConfigurationManager.AppSettings["pythonAPIUrl"];
            switch (predictionModelFuncType)
            {
                case Enums.PredictionModelFuncTypes.Regression:
                    rootPath += "regression";
                    break;
                case Enums.PredictionModelFuncTypes.PartialAutocorrelation:
                    rootPath += "Pacf";
                    break;
                case Enums.PredictionModelFuncTypes.Decomposition:
                    rootPath += "decompose";
                    break;
                case Enums.PredictionModelFuncTypes.Forecast:
                    rootPath += "ForeCast";
                    break;
                case Enums.PredictionModelFuncTypes.VARForecast:
                    rootPath += "varForecast";
                    break;
                case Enums.PredictionModelFuncTypes.VARFit:
                    rootPath += "fitVar";
                    break;
                case Enums.PredictionModelFuncTypes.mlpForecast:
                    rootPath += "mlpForecast";
                    break;
                case Enums.PredictionModelFuncTypes.regressionPreProcessing:
                    rootPath += "regressionPreProcessing";
                    break;
                default:
                    break;
            }
            return rootPath;
        }



    }
}