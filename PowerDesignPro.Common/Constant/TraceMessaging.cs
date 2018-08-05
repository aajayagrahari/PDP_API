using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Common
{
    public static class TraceMessaging
    {
        public static string EventSource = System.Configuration.ConfigurationManager.AppSettings["EventSource"].ToString();

        //public enum TraceLevel
        //{
        //    Error = 1,
        //    Verbose = 3,            
        //}
    }
}
