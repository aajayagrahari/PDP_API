using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class WelderLoadPickListDto : BaseSolutionLoadPickListDto
    {
        public IEnumerable<PickListDto> VoltagePhaseList { get; set; }

        public IEnumerable<VoltageNominalDto> VoltageNominalList { get; set; }

        public IEnumerable<VoltageSpecificDto> VoltageSpecificList { get; set; }

        public IEnumerable<WelderTypeDto> WelderTypeList { get; set; }

        public IEnumerable<PickListDto> HarmonicDeviceTypeList { get; set; }

        public IEnumerable<PickListDto> HarmonicContentList { get; set; }

        public IEnumerable<PickListDto> PFList { get; set; }

        public IEnumerable<SizeUnitsDto> SizeUnitsList { get; set; }
    }
}
