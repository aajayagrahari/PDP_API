using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class UPSLoadPickListDto : BaseSolutionLoadPickListDto
    {
        public IEnumerable<PickListDto> PhaseList { get; set; }

        public IEnumerable<PickListDto> EfficiencyList { get; set; }

        public IEnumerable<PickListDto> ChargeRateList { get; set; }

        public IEnumerable<PickListDto> PowerFactorList { get; set; }

        public IEnumerable<PickListDto> LoadLevelList { get; set; }

        public IEnumerable<PickListDto> UPSTypeList { get; set; }

        public IEnumerable<SizeUnitsDto> SizeKVAUnitsList { get; set; }

        public IEnumerable<HarmonicDeviceTypeDto> HarmonicDeviceTypeList { get; set; }

        public IEnumerable<PickListDto> HarmonicContentList { get; set; }

        public IEnumerable<PickListDto> PFList { get; set; }

        public IEnumerable<SizeUnitsDto> SizeUnitsList { get; set; }
    }
}
