namespace PowerDesignPro.BusinessProcessors.Dtos
{
    /// <summary>
    /// POCO class for Load List in the Solution Summary page
    /// </summary>
    public class BaseLoadListDto
    {
        public string LoadName { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public decimal StartingKW { get; set; }

        public decimal StartingKVA { get; set; }

        public decimal RunningKW { get; set; }

        public decimal RunningKVA { get; set; }

        /// <summary>
        /// Also refered as Harmonic Distortion Peak
        /// </summary>
        public decimal THIDMomentary { get; set; }

        /// <summary>
        /// Also refered as Harmonic Distortion Running
        /// </summary>
        public decimal THIDContinuous { get; set; }

        public decimal THIDKva { get; set; }
    }
}
