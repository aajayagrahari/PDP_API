using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class StartingMethodDto : PickListDto
    {
        public int DefaultHarmonicTypeID { get; set; }

        public int VoltageDipID { get; set; }

        public int FrequencyDipID { get; set; }

        public int MotorLoadTypeID { get; set; }
    }
}
