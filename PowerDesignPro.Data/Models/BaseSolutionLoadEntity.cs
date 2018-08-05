using System;

namespace PowerDesignPro.Data.Models
{
    /// <summary>
    /// Base entity for Loads added to the solution
    /// </summary>
    public class BaseSolutionLoadEntity : IEntity
    {
        public int ID { get; set; }

        public string Description { get; set; }

        public int LoadID { get; set; }

        public int SolutionID { get; set; }

        public int Quantity { get; set; }

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

        public DateTime CreatedDateTime { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public string ModifiedBy { get; set; }

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

        #region Navigation Properties

        public virtual Sequence Sequence { get; set; }

        public virtual VoltageNominal VoltageNominal { get; set; }

        public virtual VoltageSpecific VoltageSpecific { get; set; }

        public virtual VoltagePhase VoltagePhase { get; set; }

        public virtual VoltageDip VoltageDip { get; set; }

        public virtual VoltageDipUnits VoltageDipUnits { get; set; }

        public virtual FrequencyDip FrequencyDip { get; set; }

        public virtual FrequencyDipUnits FrequencyDipUnits { get; set; }

        public virtual Load Load { get; set; }

        public virtual Solution Solution { get; set; }

        #endregion
    }
}
