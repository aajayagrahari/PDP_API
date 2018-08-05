using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tStartingCode")]
    public class StartingCode : BasePickListEntity, IEntity
    {
        public StartingCode()
        {
            LoadDefaults = new HashSet<LoadDefaults>();
            MotorLoad = new HashSet<MotorLoad>();
            MotorType = new HashSet<MotorType>();
            MotorCalculation1 = new HashSet<MotorCalculation>();
            MotorCalculation2 = new HashSet<MotorCalculation>();
        }

        public decimal? KVAHPStarting { get; set; } 

        public string AmpsDescription { get; set; }

        public string KWMDescription { get; set; }

        public string LanguageKeyKWM { get; set; }

        public string LanguageKeyHP { get; set; }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<MotorType> MotorType { get; set; }

        public virtual ICollection<MotorLoad> MotorLoad { get; set; }

        public virtual ICollection<MotorCalculation> MotorCalculation1 { get; set; }

        public virtual ICollection<MotorCalculation> MotorCalculation2 { get; set; }
    }
}
