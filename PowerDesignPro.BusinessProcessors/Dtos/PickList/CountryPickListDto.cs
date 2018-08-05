using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class CountryPickListDto
    {
        public int ID { get; set; }

        public string Description { get; set; }

        public string CountryCode { get; set; }

        public string LanguageKey { get; set; }
    }
}
