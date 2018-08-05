namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class BaseSolutionLoadDto
    {
        public BaseSolutionLoadDto()
        {
        }
        public int ID { get; set; }

        public int LoadID { get; set; }

        public int SolutionID { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; } = 1;

        public int SequenceID { get; set; }

        public int? VoltageNominalID { get; set; }

        public int? VoltageSpecificID { get; set; }

        public int? VoltagePhaseID { get; set; }

        public int VoltageDipID { get; set; }

        public int VoltageDipUnitsID { get; set; }

        public int FrequencyDipID { get; set; }

        public int FrequencyDipUnitsID { get; set; }

        public decimal StartingLoadKva { get; set; }

        public decimal StartingLoadKw { get; set; }

        public decimal RunningLoadKva { get; set; }

        public decimal RunningLoadKw { get; set; }

        public decimal THIDMomentary { get; set; }

        public decimal THIDContinuous { get; set; }

        public decimal THIDKva { get; set; }

        public decimal HD3 { get; set; }

        public decimal HD5 { get; set; }

        public decimal HD7 { get; set; }

        public decimal HD9 { get; set; }

        public decimal HD11 { get; set; }

        public decimal HD13 { get; set; }

        public decimal HD15 { get; set; }

        public decimal HD17 { get; set; }

        public decimal HD19 { get; set; }

        public int Frequency { get; internal set; }

        public int? StartingMethodID { get; set; }

        // adding more fields
        public decimal? PFStarting { get; set; }

        public decimal? PFRunning { get; set; }

        public string SizeRunningUnits { get; set; }

        public string SizeStartingUnits { get; set; }

        public int? VoltagePhase { get; set; }

        public int? VoltageSpecific { get; set; }

        public string LoadSequenceType { get; set; }
    }
}
