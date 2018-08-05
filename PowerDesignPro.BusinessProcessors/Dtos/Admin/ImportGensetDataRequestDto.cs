using PowerDesignPro.Data.Framework.Annotations;
using System.ComponentModel.DataAnnotations;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class ImportGensetDataRequestDto
    {
        public string ProductFamily { get; set; }

        [StringLength(100)]
        public string ModelDescription { get; set; }

        [Precision(4, 1)]
        public decimal Liters { get; set; }

        public string PrimePowerAvailable { get; set; }

        public string RegulatoryFilters { get; set; }

        [Precision(4, 1)]
        public decimal FDip50 { get; set; }

        [Precision(4, 1)]
        public decimal FDip100 { get; set; }

        public int? AltitudeDeratePoint { get; set; }

        public decimal AltitudeDeratePercent { get; set; }

        public int AmbientDeratePoint { get; set; }

        public decimal AmbientDeratePercent { get; set; }

        public string AvailableFuelCode { get; set; }

        public string PMGConfigurable { get; set; }

        [Precision(5, 2)]
        public decimal SoundOpen { get; set; }

        [Precision(5, 2)]
        public decimal SoundWeather { get; set; }

        [Precision(5, 2)]
        public decimal SoundLevel1 { get; set; }

        [Precision(5, 2)]
        public decimal SoundLevel2 { get; set; }

        [Precision(5, 2)]
        public decimal SoundLevel3 { get; set; }

        public int? NG_CF_HR { get; set; }

        public int? NG_h20 { get; set; }

        public int ExhaustCFM { get; set; }

        public int ExhaustTempF { get; set; }

        [Precision(4, 2)]
        public decimal ExhaustHg { get; set; }

        [Precision(4, 1)]
        public decimal FrameFootprintLengthIn { get; set; }

        [Precision(4, 1)]
        public decimal FrameFootprintWidthIn { get; set; }

        [Precision(4, 1)]
        public decimal WeatherTotalLengthIn { get; set; }

        [Precision(4, 1)]
        public decimal SoundL2TotalLengthIn { get; set; }

        public int LPFuelCheck { get; set; }

        [Precision(4, 1)]
        public decimal SoundL1TotalLengthIn { get; set; }

        public string IsGemini { get; set; }

        [StringLength(100)]
        public string InternalDescription { get; set; }

        public int WeatherHoods { get; set; }

        public int SoundL1Hoods { get; set; }

        public int SoundL2Hoods { get; set; }

        public int PadDepth { get; set; }

        [Precision(3, 1)]
        public decimal ExhaustFlex { get; set; }

        public decimal ExhaustDual { get; set; }

        public decimal? VoltsHertzMultiplier { get; set; }

        public int Frequency { get; set; }

        public int KwStandby { get; set; }

        public int KWPrime { get; set; }

        public int KWPeak { get; set; }

        public string IsParallelable { get; set; }
    }
}
