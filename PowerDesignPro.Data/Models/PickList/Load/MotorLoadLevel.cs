using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Data.Models
{
    [Table("tMotorLoadLevel")]
    public class MotorLoadLevel : BasePickListEntity, IEntity
    {
        public MotorLoadLevel()
        {
            LoadDefaults = new HashSet<LoadDefaults>();
            MotorLoad = new HashSet<MotorLoad>();
        }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<MotorLoad> MotorLoad { get; set; }
    }
}
