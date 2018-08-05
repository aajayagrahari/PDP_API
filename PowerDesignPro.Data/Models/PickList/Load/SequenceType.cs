using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tSequenceType")]
    public class SequenceType : BasePickListEntity, IEntity
    {
        public SequenceType()
        {
            Sequences = new HashSet<Sequence>();
        }

        public virtual ICollection<Sequence> Sequences { get; set; }
    }
}
