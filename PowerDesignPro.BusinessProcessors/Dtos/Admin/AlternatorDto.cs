using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class AlternatorDto
    {
        public int ID { get; set; }

        public int AlternatorFamilyID { get; set; }

        public int FrequencyID { get; set; }

        public int VoltagePhaseID { get; set; }

        public int VoltageNominalID { get; set; }

        public string ModelDescription { get; set; }

        public int KWRating { get; set; }

        public int Percent35 { get; set; }

        public int Percent25 { get; set; }

        public int Percent15 { get; set; }

        public int KVABase { get; set; }

        public decimal SubTransientReactance { get; set; }

        public decimal? TransientReactance { get; set; }

        public decimal? SynchronousReactance { get; set; }

        public decimal? X2 { get; set; }

        public decimal? X0 { get; set; }

        public decimal? SubtransientTime { get; set; }

        public decimal? TransientTime { get; set; }

        public decimal? AmbientTemperature { get; set; }

        public int SSPU { get; set; }

        public string InternalDescription { get; set; }

        public string FrequencyDescription { get; set; }

        public string VoltageNominalDescription { get; set; }

        public string VoltagePhaseDescription { get; set; }

        public int Ordinal { get; set; }

        public string UserName { get; set; }

        public IEnumerable<PickListDto> AlternatorFamilyList { get; set; }

        public IEnumerable<PickListDto> VoltagePhaseList { get; set; }

        public IEnumerable<PickListDto> FrequencyList { get; set; }

        public IEnumerable<VoltageNominalDto> VoltageNominalList { get; set; }
    }
}
