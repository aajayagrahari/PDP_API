using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    /// <summary>
    /// DTO for Load List in the Solution Setup screen
    /// </summary>
    public class LoadsDto : PickListDto
    {
        public int LoadFamilyID { get; set; }

        public bool IsDefaultSelection { get; set; }

        public bool Active { get; set; }

        public string LoadFamily { get; set; }
    }
}
