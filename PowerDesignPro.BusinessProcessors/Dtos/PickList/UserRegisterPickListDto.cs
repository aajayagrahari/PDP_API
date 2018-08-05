using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class UserRegisterPickListDto
    {
        public IEnumerable<StatePickListDto> StateList { get; set; }

        public IEnumerable<CountryPickListDto> CountryList { get; set; }

        public IEnumerable<PickListDto> UserCategoryList { get; set; }

        //public IEnumerable<PickListDto> BrandList { get; set; }
    }
}
