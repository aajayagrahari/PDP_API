using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class SharedProjectsDto
    {
        public int SharedProjectID { get; set; }

        public string SharedProjectName { get; set; } = "";

        public string SharerUserName { get; set; } = "";

        public IEnumerable<string> SharedSolutionNames { get; set; }

        public int ProjectID { get; set; }

        public string RecipientEmail { get; set; }

        public string Notes { get; set; }

        public IEnumerable<int> SelectedSolutionList { get; set; }

        public string Language { get; set; }
    }
}
