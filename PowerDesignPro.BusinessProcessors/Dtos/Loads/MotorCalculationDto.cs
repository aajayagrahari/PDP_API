using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class MotorCalculationDto
    {
        public int ID { get; set; }

        public decimal HP { get; set; }

        public decimal kWm { get; set; }

        public decimal KVARunning { get; set; }

        public decimal PFRunning { get; set; }

        public decimal PFStarting { get; set; }

        public decimal kVAHPStartingNema { get; set; }

        public decimal KVAHPStartingIEC { get; set; }

        public int? StartingCodeIDNema { get; set; }

        public int? StartingCodeIDIEC { get; set; }

        public int CalcReferenceIEC { get; set; }
    }
}
