using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class LightingTypeDto : PickListDto
    {
        public int RunningPFID { get; set; }

        public int HarmonicDeviceTypeID { get; set; }

        public int HarmonicContentID { get; set; }
    }
}
