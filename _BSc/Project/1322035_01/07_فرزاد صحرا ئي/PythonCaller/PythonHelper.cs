using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;

namespace PythonCaller
{
    internal class PythonHelper
    {
        public PyConverter GetConverter()
        {
          
            //using (Py.GIL())
            //{
                var converter = new PyConverter();
                converter.AddListType();
                converter.Add(new StringType());
                converter.Add(new Int64Type());
                converter.Add(new Int32Type());
                converter.Add(new FloatType());
                converter.Add(new DoubleType());
                converter.AddDictType<object, object>();
                return converter;

            //}
            
        }
    }
}
