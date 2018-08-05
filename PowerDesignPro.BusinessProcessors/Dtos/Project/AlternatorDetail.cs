using PowerDesignPro.Data.Models;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class AlternatorDetail
    {
        public Alternator Alternator { get; internal set; }

        public int AlternatorID { get; internal set; }

        public decimal SubtransientReactance1000 { get; internal set; }

        public decimal SubtransientReactanceCorrected { get; internal set; }

        public decimal KWDerated { get; internal set; }

        public decimal TransientKWVDip_10 { get; internal set; }

        public decimal TransientKWVDip_125 { get; internal set; }

        public decimal TransientKWVDip_15 { get; internal set; }

        public decimal TransientKWVDip_175 { get; internal set; }

        public decimal TransientKWVDip_20 { get; internal set; }

        public decimal TransientKWVDip_225 { get; internal set; }

        public decimal TransientKWVDip_25 { get; internal set; }

        public decimal TransientKWVDip_275 { get; internal set; }

        public decimal TransientKWVDip_30 { get; internal set; }

        public decimal TransientKWVDip_325 { get; internal set; }

        public decimal TransientKWVDip_35 { get; internal set; }

        public double SKVAMultiplier { get; internal set; }
    }
}
