using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class GrantEditAccessResponseDto
    {
        public int ProjectID { get; set; }

        public int SolutionID { get; set; }

        public string SolutionName { get; set; }

        public string ProjectName { get; set; }

        public string SolutionComments { get; set; }

        public string RecipientEmail { get; set; }
    }
}
