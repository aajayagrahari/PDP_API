using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class ACLoadPickListDto : BaseSolutionLoadPickListDto
    {
        public IEnumerable<SizeUnitsDto> CoolingUnitsList { get; set; }

        public IEnumerable<PickListDto> CompressorsList { get; set; }

        public IEnumerable<PickListDto> CoolingLoadList { get; set; }

        public IEnumerable<PickListDto> ReheatLoadList { get; set; }
    }
}
