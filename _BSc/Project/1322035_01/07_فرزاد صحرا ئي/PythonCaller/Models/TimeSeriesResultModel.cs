using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonCaller.Models
{
    public class TimeSeriesResultModel
    {
        public Dictionary<object, object> Predict { get; set; }
        public Dictionary<object, object> Forcast { get; set; }

        public TimeSeriesResultModel(Dictionary<object, object> predict, Dictionary<object, object> forcast)
        {
            Predict = predict;
            Forcast = forcast;
        }

      
    }
}
