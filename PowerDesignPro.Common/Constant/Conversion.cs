using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Common
{
    public static class Conversion
    {
        public static decimal GetRoundedDecimal(decimal input, int precision)
        {
            return decimal.Round(input, precision, MidpointRounding.AwayFromZero);
        }

        public static IDictionary<string, decimal> GetConversionFactorList()
        {
            IDictionary<string, decimal> conversionFactorList = new Dictionary<string, decimal>();

            conversionFactorList = (ConfigurationManager.GetSection("conversionFactor") as System.Collections.Hashtable)
                 .Cast<System.Collections.DictionaryEntry>()
                 .ToDictionary(n => n.Key.ToString(), n => Convert.ToDecimal(n.Value));

            return conversionFactorList;
        }
    }
}
