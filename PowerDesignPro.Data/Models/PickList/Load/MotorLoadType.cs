using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tMotorLoadType")]
    public class MotorLoadType : BasePickListEntity, IEntity
    {
        public MotorLoadType()
        {
            LoadDefaults = new HashSet<LoadDefaults>();
            MotorLoad = new HashSet<MotorLoad>();
            StartingMethod = new HashSet<StartingMethod>();
            HarmonicDeviceType = new HashSet<HarmonicDeviceType>();
        }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<MotorLoad> MotorLoad { get; set; }

        public virtual ICollection<StartingMethod> StartingMethod { get; set; }

        public virtual ICollection<HarmonicDeviceType> HarmonicDeviceType { get; set; }
    }
}
