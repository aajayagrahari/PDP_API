using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class SolutionApplicationDto
    {
        public int ID { get; set; }

        public string Description { get; set; }

        public string Value { get; set; }

        public int Ordinal { get; set; }

        public string UserName { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public bool Active { get; set; }
    }
}
