using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tWelderType")]
    public class WelderType : BasePickListEntity, IEntity
    {
        public WelderType()
        {
            LoadDefaults = new HashSet<LoadDefaults>();
            WelderLoad = new HashSet<WelderLoad>();
        }

        public int HarmonicDeviceTypeID { get; set; }

        public virtual HarmonicDeviceType HarmonicDeviceType { get; set; }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<WelderLoad> WelderLoad { get; set; }

    }
}
