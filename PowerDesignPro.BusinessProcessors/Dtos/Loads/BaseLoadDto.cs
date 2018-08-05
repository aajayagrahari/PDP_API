using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class BaseLoadDto
    {
        public BaseLoadDto()
        {
            Loads = new HashSet<LoadsDto>();
        }

        public int SolutionId { get; set; }

        public string SolutionName { get; set; }

        public IEnumerable<LoadsDto> Loads { get; set; }
    }
}
