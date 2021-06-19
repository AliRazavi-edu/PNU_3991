using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSeriseUI.Common
{
    public class Enums
    {
        public enum PredictionModelFuncTypes : byte
        {
            Regression = 1,
            PartialAutocorrelation = 2,
            Decomposition = 3,
            Forecast = 4,
            VARForecast = 5,
            VARFit = 6,
            mlpForecast = 7,
            regressionPreProcessing = 8
        }

        public enum ForecastsType : short
        {
            sarima = 1,
            arima = 2,
            arma = 3,

        }

        public static Dictionary<int,string> GetForecastsTypeBindable()
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            result.Add(ForecastsType.sarima.GetHashCode(), "SARIMA");
            result.Add(ForecastsType.arima.GetHashCode(), "ARIMA");
            result.Add(ForecastsType.arma.GetHashCode(), "ARMA");
            return result;
        }
    }
}