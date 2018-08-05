using PowerDesignPro.Data.Framework.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    public class ACLoad : BaseSolutionLoadEntity
    {
        [Precision(10,2)]
        public decimal? Cooling { get; set; }

        public int? CoolingUnitsID { get; set; }

        public int? CompressorsID { get; set; }

        public int? CoolingLoadID { get; set; }

        public int? ReheatLoadID { get; set; }

        public virtual SizeUnits CoolingUnits { get; set; }

        public virtual Compressors Compressors { get; set; }

        public virtual CoolingLoad CoolingLoad { get; set; }

        public virtual ReheatLoad ReheatLoad { get; set; }
    }
}
