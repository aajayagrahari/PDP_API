using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Common
{
    public static class VDipFDip
    {
        public static decimal ConvertVoltageDip(int voltageSpecific, decimal voltageDip, int outUnitsID)
        {
            if (outUnitsID == (int)VoltageDipUnitsEnum.Percent)
                return voltageDip / voltageSpecific;
            else
                return Math.Round(voltageDip * voltageSpecific);
        }

        public static decimal ConvertFrequencyDip(int frequency, decimal frequencyDip, int outUnitsID)
        {
            if (outUnitsID == (int)FrequencyDipUnitsEnum.Percent)
                return frequencyDip / frequency;
            else
                return Math.Ceiling(frequencyDip * frequency);
        }
    }
}
