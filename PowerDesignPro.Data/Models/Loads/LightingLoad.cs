using PowerDesignPro.Data.Framework.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Data.Models
{
    [Table("tLightingLoad")]
    public class LightingLoad : BaseSolutionLoadEntity
    {
        [Precision(10,2)]
        public decimal? SizeRunning { get; set; }

        public int? SizeRunningUnitsID { get; set; }

        public int? LightingTypeID { get; set; }

        public int? RunningPFID { get; set; }

        public int? HarmonicDeviceTypeID { get; set; }

        public int? HarmonicContentID { get; set; }

        public virtual SizeUnits SizeRunningUnits { get; set; }

        public virtual PF RunningPF { get; set; }

        public virtual HarmonicDeviceType HarmonicDeviceType { get; set; }

        public virtual HarmonicContent HarmonicContent { get; set; }

        public virtual LightingType LightingType { get; set; }
    }
}
