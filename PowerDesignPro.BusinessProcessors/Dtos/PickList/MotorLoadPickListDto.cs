using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class MotorLoadPickListDto : BaseSolutionLoadPickListDto
    {
        public IEnumerable<PickListDto> VoltagePhaseList { get; set; }

        public IEnumerable<VoltageNominalDto> VoltageNominalList { get; set; }

        public IEnumerable<VoltageSpecificDto> VoltageSpecificList { get; set; }

        public IEnumerable<SizeUnitsDto> SizeUnitsList { get; set; }

        public IEnumerable<HarmonicDeviceTypeDto> HarmonicDeviceTypeList { get; set; }

        public IEnumerable<HarmonicContentDto> HarmonicContentList { get; set; }

        public IEnumerable<PickListDto> MotorLoadLevelList { get; set; }

        public IEnumerable<PickListDto> MotorLoadTypeList { get; set; }

        public IEnumerable<PickListDto> MotorTypeList { get; set; }

        public IEnumerable<PickListDto> StartingCodeList { get; set; }

        public IEnumerable<PickListDto> StartingMethodList { get; set; }

        public IEnumerable<ConfigurationInputDto> ConfigurationInputList { get; set; }

        public IEnumerable<MotorCalculationDto> MotorCalculationList { get; set; }
    }
}
