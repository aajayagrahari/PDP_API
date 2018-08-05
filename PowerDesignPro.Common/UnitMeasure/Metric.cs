using PowerDesignPro.Common.Constant;

namespace PowerDesignPro.Common.UnitMeasure
{
    public class Metric : IUnitMeasure
    {
        public Metric()
        {
        }

        public int UnitID => (int)UnitEnum.Metric;

        public decimal BasePressureFactor => (decimal)4.01865;

        public decimal BaseLengthOfRunFactor => (decimal)3.2808;

        public decimal BasePipeSizeFactor => (decimal)0.0394;

        public string PressureUnitText => "kPa";

        public string LengthOfRunUnitText => "m";

        public string PipeSizeUnitText => "mm";
    }
}
