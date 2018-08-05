using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
   public class ReportModel
    {
        public ReportModel()
        {
            LanguageCode = "en";
        }
        public string LanguageCode { get; set; }
        public int ProjectId { get; set; }
        public int SolutionId { get; set; }
        public string UserID { get; set; }
        public  string brand { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public HarmonicAnalysisDto HarmonicAnalysis { get; set; }
        public string HarmicAnalysisImageUrl { get; set; } 
    }
}
