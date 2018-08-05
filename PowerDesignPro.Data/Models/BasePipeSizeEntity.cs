using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Data.Models
{
    public class BasePipeSizeEntity: BaseEntity, IEntity
    {
        public decimal PipeSize { get; set; }

        public int Ordinal { get; set; }
    }
}
