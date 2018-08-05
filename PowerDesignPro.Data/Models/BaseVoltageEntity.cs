using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Data.Models
{
    public class BaseVoltageEntity : BasePickListEntity
    {
        public int VoltagePhaseID { get; set; }

        public int FrequencyID { get; set; }

        public bool IsDefaultSelection { get; set; }

        public bool IsForLoads { get; set; }

        #region NavigationProperties

        public virtual VoltagePhase VoltagePhase { get; set; }

        public virtual Frequency Frequency { get; set; }

        #endregion
    }
}
