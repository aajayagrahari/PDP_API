using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class DashboardSharedProjectDetailDto : BaseDto
    {
        public string ProjectName { get; set; }

        public string CreatedDateTime { get; set; }

        public string ModifiedDateTime { get; set; }

        public string SharedDateTime { get; set; }

        public string SharedUser { get; set; }
    }
}
