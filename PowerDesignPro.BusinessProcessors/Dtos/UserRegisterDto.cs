using System;
using System.ComponentModel.DataAnnotations;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class UserRegisterDto
    {
        public UserRegisterDto()
        {
            UserRegisterPickListDto = new UserRegisterPickListDto();
        }

        [Required]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Invalid email address")]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Mobile { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public int? StateID { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public int? CountryID { get; set; }

        [Required]
        public int UserCategoryID { get; set; }

        public string CustomerNo { get; set; }

        [Required]
        public int BrandID { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public UserRegisterPickListDto UserRegisterPickListDto { get; set; }

        public string Language { get; set; }
    }
}
