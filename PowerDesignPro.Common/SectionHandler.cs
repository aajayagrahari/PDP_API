using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Common
{
    public class SectionHandler : NameValueSectionHandler
    {
        public static string GetSectionValue(string section, string key, string defaultValue)
        {
            NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection(section);
            if (config != null)
            {

                if (!string.IsNullOrEmpty(config[key]))
                {
                    return config[key];
                }
                else
                {
                    return defaultValue;
                }
            }
            else
            {
                return defaultValue;
            }

        }
    }
}
