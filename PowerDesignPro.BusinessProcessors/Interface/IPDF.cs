using PowerDesignPro.BusinessProcessors.Dtos;
using System.IO;

namespace PowerDesignPro.BusinessProcessors.Interface
{
    public interface IPDF
    {
        bool WritePDF(HarmonicAnalysisDto harmonicAnalysisDto);
        MemoryStream GeneratePDF(ReportModel reportModel);
        void DeleteImage();
    }
}
