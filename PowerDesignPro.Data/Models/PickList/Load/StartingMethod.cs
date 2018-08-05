using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tStartingMethod")]
    public class StartingMethod : BasePickListEntity, IEntity
    {
        public StartingMethod()
        {
            LoadDefaults = new HashSet<LoadDefaults>();
            HarmonicDeviceTypes = new HashSet<HarmonicDeviceType>();
            HarmonicContent = new HashSet<HarmonicContent>();
            ConfigurationInput = new HashSet<ConfigurationInput>();
            MotorLoad = new HashSet<MotorLoad>();
        }
        
        public int DefaultHarmonicTypeID { get; set; }

        public int VoltageDipID { get; set; }

        public int FrequencyDipID { get; set; }

        public int MotorLoadTypeID { get; set; }

        public virtual VoltageDip VoltageDip { get; set; }

        public virtual FrequencyDip FrequencyDip { get; set; }

        public virtual MotorLoadType MotorLoadType { get; set; }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<HarmonicDeviceType> HarmonicDeviceTypes { get; set; }

        public virtual ICollection<ConfigurationInput> ConfigurationInput { get; set; }

        public virtual ICollection<HarmonicContent> HarmonicContent { get; set; }

        public virtual ICollection<MotorLoad> MotorLoad { get; set; }
    }
}
