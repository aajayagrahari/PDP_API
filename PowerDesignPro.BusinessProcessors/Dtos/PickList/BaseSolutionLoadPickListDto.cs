using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class BaseSolutionLoadPickListDto
    {
        public IEnumerable<PickListDto> SequenceList { get; set; }

        public IEnumerable<PickListDto> VoltageDipList { get; set; }

        public IEnumerable<PickListDto> VoltageDipUnitsList { get; set; }

        public IEnumerable<PickListDto> FrequencyDipList { get; set; }

        public IEnumerable<PickListDto> FrequencyDipUnitsList { get; set; }
    }
}
