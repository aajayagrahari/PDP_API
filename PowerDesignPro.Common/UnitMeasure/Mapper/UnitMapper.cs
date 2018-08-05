using System;

namespace PowerDesignPro.Common.UnitMeasure.Mapper
{
    /// <summary>
    /// UNitMapper to convert values from one unit to other...
    /// </summary>
    public class UnitMapper
    {
        /// <summary>
        /// Converts the pressure.
        /// </summary>
        /// <param name="fromUnit">From unit.</param>
        /// <param name="toUnit">To unit.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static decimal ConvertPressure(IUnitMeasure fromUnit, IUnitMeasure toUnit, decimal value, int precision = 2)
        {
            return Conversion.GetRoundedDecimal((value * fromUnit.BasePressureFactor) / toUnit.BasePressureFactor, precision);
        }

        /// <summary>
        /// Converts the length of run.
        /// </summary>
        /// <param name="fromUnit">From unit.</param>
        /// <param name="toUnit">To unit.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static decimal ConvertLengthOfRun(IUnitMeasure fromUnit, IUnitMeasure toUnit, decimal value, int precision = 2)
        {
            return Conversion.GetRoundedDecimal((value * fromUnit.BaseLengthOfRunFactor) / toUnit.BaseLengthOfRunFactor, precision);
        }

        public static decimal ConvertPipeSize(IUnitMeasure fromUnit, IUnitMeasure toUnit, decimal value, int precision = 2)
        {
            return Conversion.GetRoundedDecimal((value * fromUnit.BasePipeSizeFactor) / toUnit.BasePipeSizeFactor, precision);
        }
    }
}
