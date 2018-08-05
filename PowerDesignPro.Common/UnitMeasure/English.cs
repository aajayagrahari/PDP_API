using PowerDesignPro.Common.Constant;

namespace PowerDesignPro.Common.UnitMeasure
{
    public class English : IUnitMeasure
    {
        public English()
        {
        }

        public int UnitID => (int)UnitEnum.English;

        public decimal BasePressureFactor => (decimal)1.00;

        public decimal BaseLengthOfRunFactor => (decimal)1.00;

        public decimal BasePipeSizeFactor => (decimal)1.00;

        public string PressureUnitText => "inches of water";

        public string LengthOfRunUnitText => "ft";

        public string PipeSizeUnitText => "in";
    }
}
