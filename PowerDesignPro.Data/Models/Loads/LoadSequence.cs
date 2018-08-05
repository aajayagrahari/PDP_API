using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models.Loads
{
    [Table("tLoadSequence")]
    public class LoadSequence : BaseEntity, IEntity
    {
        public int SolutionID { get; set; }

        public int SequenceID { get; set; }

        public virtual Solution Solution { get; set; }

        public virtual Sequence Sequence { get; set; }
    }
}
