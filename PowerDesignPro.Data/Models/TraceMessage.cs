using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Data.Models
{
    [Table("tTraceMessages")]
    public class TraceMessage : IEntity
    {
        public int ID { get; set; }

        [MaxLength(50)]
        public string EventSource { get; set; }

        public DateTime MessageDateTime { get; set; }

        public int TraceLevel { get; set; }

        [MaxLength(255)]
        public string Topic { get; set; }

        [MaxLength(255)]
        public string Context { get; set; }

        [MaxLength(255)]
        public string Title { get; set; }

        public string MessageText { get; set; }
    }
}
