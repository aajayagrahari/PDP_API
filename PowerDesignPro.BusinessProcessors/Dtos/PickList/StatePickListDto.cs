using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class StatePickListDto
    {
        public int ID { get; set; }

        public int CountryID { get; set; }

        public string Description { get; set; }

        public string LanguageKey { get; set; }

        public string StateCode { get; set; }
    }
}
