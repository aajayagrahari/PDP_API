using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tMotorType")]
    public class MotorType : BasePickListEntity, IEntity
    {
        public MotorType()
        {
            LoadDefaults = new HashSet<LoadDefaults>();
            MotorLoad = new HashSet<MotorLoad>();
        }

        public int StartingCodeID { get; set; }

        public virtual StartingCode StartingCode { get; set; }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<MotorLoad> MotorLoad { get; set; }
    }
}
