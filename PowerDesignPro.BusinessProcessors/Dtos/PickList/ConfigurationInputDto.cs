using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class ConfigurationInputDto : PickListDto
    {
        public int StartingMethodID { get; set; }

        public decimal sKVAMultiplierOverride { get; set; }

        public decimal sKWMultiplierOverride { get; set; }

        public decimal rKWMultiplierOverride { get; set; }

        public bool IsDefaultSelection { get; set; }
    }
}
