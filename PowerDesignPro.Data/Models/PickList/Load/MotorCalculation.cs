using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tMotorCalculation")]
    public class MotorCalculation : IEntity
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

        public virtual StartingCode StartingCodeNema { get; set; }

        public virtual StartingCode StartingCodeIEC { get; set; }
    }
}


