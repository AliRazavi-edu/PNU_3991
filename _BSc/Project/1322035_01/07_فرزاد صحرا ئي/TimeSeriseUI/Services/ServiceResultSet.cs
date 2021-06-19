using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSeriseUI.Services
{
    public class ServiceResultSet
    {
        public  string Message { get; private set; }
        public  object Data { get; private set; }
        public bool IsSuccessful { get; internal set; }

        private ServiceResultSet(string message, object data,bool isSuccessed)
        {
            Message = message;
            Data = data;
            IsSuccessful = isSuccessed;
        }

        public static ServiceResultSet GetUnSuccessful(string message)
        {
            return new ServiceResultSet(message, null,false);
        }

        public static ServiceResultSet GetSuccessful(string message, object data)
        {
            return new ServiceResultSet(message, data,true);
        }
    }
}