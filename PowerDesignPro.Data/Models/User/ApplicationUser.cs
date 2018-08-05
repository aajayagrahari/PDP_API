using Microsoft.AspNet.Identity.EntityFramework;
using PowerDesignPro.Data.Models.User;
using System;
using System.Collections.Generic;

namespace PowerDesignPro.Data.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Projects = new HashSet<Project>();
            UserDefaultSolutionSetups = new HashSet<UserDefaultSolutionSetup>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CompanyName { get; set; }

        public int UserCategoryID { get; set; }

        public string CustomerNumber { get; set; }

        //public int BrandID { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public int? StateID { get; set; }

        public string ZipCode { get; set; }

        public int? CountryID { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public DateTime? LastLoginDateTime { get; set; }

        public DateTime? LastPasswordChangedDateTime { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<UserDefaultSolutionSetup> UserDefaultSolutionSetups { get; set; }

        public virtual UserCategory UserCategory { get; set; }

        //public virtual Brand Brand { get; set; }

        public virtual State State { get; set; }

        public virtual Country Country { get; set; }
    }
}