using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tCompressors")]
    public class Compressors : BasePickListEntity, IEntity
    {
        public Compressors()
        {
            LoadDefaults = new HashSet<LoadDefaults>();
            ACLoad = new HashSet<ACLoad>();
        }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<ACLoad> ACLoad { get; set; }
    }
}
