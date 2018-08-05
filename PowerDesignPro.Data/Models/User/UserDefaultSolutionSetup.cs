using System;

namespace PowerDesignPro.Data.Models
{
    public class UserDefaultSolutionSetup : BaseSolutionSetupEntity, IEntity
    {
        public string UserID { get; set; }

        public bool IsGlobalDefaults { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
