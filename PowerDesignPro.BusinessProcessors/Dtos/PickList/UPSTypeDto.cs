using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class UPSTypeDto : PickListDto
    {
        public int PhaseID { get; set; }

        public int EfficiencyID { get; set; }

        public int HarmonicDeviceTypeID { get; set; }

        public int HarmonicContentID { get; set; }
    }
}
