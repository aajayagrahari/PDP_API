using PowerDesignPro.BusinessProcessors.Interface;
using PowerDesignPro.Data.Framework.Interface;
using PowerDesignPro.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Processors
{
    public class TraceMessageProcessor : ITraceMessage
    {
        private readonly IEntityBaseRepository<TraceMessage> _traceMessageRepository;

        public TraceMessageProcessor(IEntityBaseRepository<TraceMessage> traceMessageRepository)
        {
            _traceMessageRepository = traceMessageRepository;
        }

        public void WriteTrace(string eventSource, TraceLevel traceLevel, string topic, string context, string title, string messageText)
        //public void WriteTrace(TraceMessage traceMessage)
        {
            var traceMessage = new TraceMessage
            {
                EventSource = eventSource,
                MessageDateTime = DateTime.UtcNow,
                TraceLevel = (int)traceLevel,
                Topic = topic,
                Context = context,
                Title = title,
                MessageText = messageText
            };
            _traceMessageRepository.Add(traceMessage);
            _traceMessageRepository.Commit();
        }
    }
}
