using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Data.Models
{
    [Table("tConfigurationInput")]
    public class ConfigurationInput : BasePickListEntity, IEntity
    {
        public ConfigurationInput()
        {
            LoadDefaults = new HashSet<LoadDefaults>();
            MotorLoad = new HashSet<MotorLoad>();
        }

        public int StartingMethodID { get; set; }

        public decimal sKVAMultiplierOverride { get; set; }

        public decimal sKWMultiplierOverride { get; set; }

        public decimal rKWMultiplierOverride { get; set; }

        public bool IsDefaultSelection { get; set; }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<MotorLoad> MotorLoad { get; set; }

        public virtual StartingMethod StartingMethod { get; set; }
    }
}
