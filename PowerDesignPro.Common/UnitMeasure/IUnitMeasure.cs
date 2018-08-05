namespace PowerDesignPro.Common.UnitMeasure
{
    public interface IUnitMeasure
    {
        int UnitID
        {
            get;
        }

        decimal BasePressureFactor
        {
            get; 
        }

        decimal BaseLengthOfRunFactor
        {
            get;
        }

        decimal BasePipeSizeFactor
        {
            get;
        }

        string PressureUnitText
        {
            get;
        }

        string LengthOfRunUnitText
        {
            get;
        }

        string PipeSizeUnitText
        {
            get;
        }
    }
}
