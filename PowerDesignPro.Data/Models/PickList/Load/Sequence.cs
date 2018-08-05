using PowerDesignPro.Data.Models.Loads;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tSequence")]
    public class Sequence : BasePickListEntity, IEntity
    {
        public Sequence()
        {
            LoadDefaults = new HashSet<LoadDefaults>();
            LoadSequence = new HashSet<LoadSequence>();
        }

        public int SequenceTypeID { get; set; }

        public virtual SequenceType SequenceType { get; set; }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<LoadSequence> LoadSequence { get; set; }

    }
}
