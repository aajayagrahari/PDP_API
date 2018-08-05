using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class HarmonicDeviceTypeDto : PickListDto
    {
        public int HarmonicContentID { get; set; }

        public decimal? HD3 { get; set; }

        public decimal? HD5 { get; set; }

        public decimal? HD7 { get; set; }

        public decimal? HD9 { get; set; }

        public decimal? HD11 { get; set; }

        public decimal? HD13 { get; set; }

        public decimal? HD15 { get; set; }

        public decimal? HD17 { get; set; }

        public decimal? HD19 { get; set; }

        public decimal? KVAMultiplier { get; set; }

        public decimal? KWMultiplier { get; set; }

        public string StartingMethodDesc { get; set; }

        public string HarmonicContentDesc { get; set; }

        public int LoadLimit { get; set; }

        public int StartingMethodID { get; set; }

        public bool IsDefaultSelection { get; set; }

        public string UserName { get; set; }

        public int MotorLoadTypeID { get; set; }

        //public DateTime CreatedDateTime { get; set; }

        //public DateTime ModifiedDateTime { get; set; }

        public bool Active { get; set; }

        public IEnumerable<PickListDto> StartingMethodList { get; set; }

        public IEnumerable<HarmonicContentDto> HarmonicContentList { get; set; }
    }
}
