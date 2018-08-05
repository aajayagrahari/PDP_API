using PowerDesignPro.BusinessProcessors.Dtos.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class GeneratorDto
    {
        public int ID { get; set; }

        public int ProductFamilyID { get; set; }

        public string ModelDescription { get; set; }

        public decimal Liters { get; set; }

        public int KwStandby { get; set; }

        public int KWPrime { get; set; }

        public int KWPeak { get; set; }

        public bool PrimePowerAvailable { get; set; }

        public int FrequencyID { get; set; }

        public decimal FDip50 { get; set; }

        public decimal FDip100 { get; set; }

        public int? AltitudeDeratePoint { get; set; }

        public decimal AltitudeDeratePercent { get; set; }

        public int AmbientDeratePoint { get; set; }

        public decimal AmbientDeratePercent { get; set; }

        public string AvailableFuelCode { get; set; }

        public bool PMGConfigurable { get; set; }

        public decimal SoundOpen { get; set; }

        public decimal SoundWeather { get; set; }

        public decimal SoundLevel1 { get; set; }

        public decimal SoundLevel2 { get; set; }

        public decimal SoundLevel3 { get; set; }

        public int ExhaustCFM { get; set; }

        public int ExhaustTempF { get; set; }

        public decimal ExhaustHg { get; set; }

        public decimal FrameFootprintLengthIn { get; set; }

        public decimal FrameFootprintWidthIn { get; set; }

        public decimal WeatherTotalLengthIn { get; set; }

        public decimal SoundL2TotalLengthIn { get; set; }

        public bool LPFuelCheck { get; set; }

        public decimal SoundL1TotalLengthIn { get; set; }

        public bool IsGemini { get; set; }

        public int WeatherHoods { get; set; }

        public int PadDepth { get; set; }

        public int SoundL1Hoods { get; set; }

        public int SoundL2Hoods { get; set; }

        public string InternalDescription { get; set; }

        public decimal ExhaustFlex { get; set; }

        public bool ExhaustDual { get; set; }

        public int? NG_CF_HR { get; set; }

        public int? NG_h20 { get; set; }

        public decimal? VoltsHertzMultiplier { get; set; }

        public string Description { get; set; }

        public string UserName { get; set; }

        public bool IsParallelable { get; set; }

        public IEnumerable<PickListDto> FrequencyList { get; set; }

        public IEnumerable<VoltageNominalDto> VoltageNominalList { get; set; }

        public IEnumerable<FuelTypeDto> FuelTypeList { get; set; }

        public IEnumerable<RegulatoryFilterDto> RegulatoryFilterList { get; set; }

        public IEnumerable<PickListDto> SelectedVoltageNominalList { get; set; }

        public IEnumerable<PickListDto> SelectedRegulatoryFilterList { get; set; }

        public IEnumerable<PickListDto> SelectedAlternatorList { get; set; }

        public IEnumerable<PickListDto> ProductFamilyList { get; set; }

        public IEnumerable<AlternatorFamilyTreeDto> AlternatorFamilyTreeList { get; set; }
    }
}
