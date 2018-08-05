using PowerDesignPro.Data.Framework.Annotations;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class ImportAlternatorDataRequestDto
    {
        public string ModelDescription { get; set; }

        public int KWRating { get; set; }

        public int Percent35 { get; set; }

        public int Percent25 { get; set; }

        public int Percent15 { get; set; }

        public int KVABase { get; set; }

        [Precision(5, 3)]
        public decimal SubTransientReactance { get; set; }

        [Precision(4, 2)]
        public decimal? TransientReactance { get; set; }

        [Precision(4, 2)]
        public decimal? SynchronousReactance { get; set; }

        [Precision(4, 2)]
        public decimal? X2 { get; set; }

        [Precision(4, 2)]
        public decimal? X0 { get; set; }

        [Precision(4, 2)]
        public decimal? SubtransientTime { get; set; }

        [Precision(4, 2)]
        public decimal? TransientTime { get; set; }

        [Precision(4, 2)]
        public decimal? AmbientTemperature { get; set; }

        public int SSPU { get; set; }

        public string InternalDescription { get; set; }

        public string AlternatorFamily { get; set; }

        public int Frequency { get; set; }

        public string VoltagePhase { get; set; }

        public int VoltageNominal { get; set; }
    }
}
