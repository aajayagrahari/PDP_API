using PowerDesignPro.Data.Framework.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tGasPipingPipeSize")]
    public class GasPipingPipeSize : BasePipeSizeEntity
    {
        public GasPipingPipeSize()
        {
            GasPipingSolutions = new HashSet<GasPipingSolution>();
        }

        [Precision(10,4)]
        public decimal Diameter { get; set; }

        [Precision(10, 4)]
        public decimal Factor45 { get; set; }

        [Precision(10, 4)]
        public decimal Factor90 { get; set; }

        [Precision(10, 4)]
        public decimal Tee { get; set; }

        public virtual ICollection<GasPipingSolution> GasPipingSolutions { get; set; }
    }
}
