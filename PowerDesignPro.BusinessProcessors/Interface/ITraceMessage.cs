using PowerDesignPro.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Interface
{
    public interface ITraceMessage
    {
        void WriteTrace(string EventSource, TraceLevel TraceLevel, string Topic, string Context, string Title, string MessageText);

        //void WriteTrace(TraceMessage traceMessage);
    }
}
