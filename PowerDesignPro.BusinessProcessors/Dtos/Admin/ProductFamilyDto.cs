using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class ProductFamilyDto 
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public bool IsDomestic { get; set; }

        public bool IsGemini { get; set; }

        public int Priority { get; set; }

        public int BrandID { get; set; }

        public string Username { get; set; }

        public string LanguageKey { get; set; }

        public bool Active { get; set; }

        public IEnumerable<PickListDto> BrandList { get; set; }
    }
}
