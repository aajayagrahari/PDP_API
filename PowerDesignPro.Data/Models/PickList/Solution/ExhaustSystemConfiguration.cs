using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Data.Models
{
    [Table("tExhaustSystemConfiguration")]
    public class ExhaustSystemConfiguration: BasePickListEntity
    {
        public ExhaustSystemConfiguration()
        {
            ExhaustPipingSolutions = new HashSet<ExhaustPipingSolution>();
        }

        public virtual ICollection<ExhaustPipingSolution> ExhaustPipingSolutions { get; set; }
    }
}
