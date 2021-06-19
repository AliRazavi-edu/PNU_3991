using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;
using PythonCaller.Models;

namespace PythonCaller
{
    public class TimeSeries
    {

        private readonly string _pythonFilePaths;

        public TimeSeries(string pythonFilePaths)
        {
            _pythonFilePaths = pythonFilePaths;
        }
        public TimeSeriesResultModel GetForcasting(Dictionary<string, decimal> data)
        {

            try
            {



                dynamic predictResult;

                var values = new Dictionary<string, List<string>>
                {
                    {"Date", data.Keys.Select(x => x.ToString()).ToList()},
                    {"riders", data.Values.Select(x => x.ToString()).ToList()}
                };
                dynamic clrPredict;
                dynamic clrForecast;

                var v = PythonEngine.Version;
                using (Py.GIL())
                {
                    var converter = (new PythonHelper()).GetConverter();
                    dynamic syspy = Py.Import("sys");
                    syspy.path.append(_pythonFilePaths);
                    dynamic ps = Py.Import("TimeSeries");
                    predictResult = ps.main(values);
                    clrPredict = converter.ToClr(predictResult[0]);
                    clrForecast = converter.ToClr(predictResult[1]);

                }

                return new TimeSeriesResultModel(clrPredict, clrForecast);
            }
            catch (Exception e)
            {

                //Todo Handle Errors
                throw e;
            }
        }


    }
}

