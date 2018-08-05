using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tSizeUnits")]
    public class SizeUnits : BasePickListEntity, IEntity
    {
        public SizeUnits()
        {
            LoadDefaultsSizeRunning = new HashSet<LoadDefaults>();
            LoadDefaultsSizeStarting = new HashSet<LoadDefaults>();
            LoadDefaultsSizeCooling = new HashSet<LoadDefaults>();
            LoadDefaultsSizeKVA = new HashSet<LoadDefaults>();

            BasicLoad1 = new HashSet<BasicLoad>();
            BasicLoad2 = new HashSet<BasicLoad>();
            ACLoads = new HashSet<ACLoad>();
            UPSLoads = new HashSet<UPSLoad>();
        }

        public int LoadFamilyID { get; set; }

        public virtual LoadFamily LoadFamily { get; set; }

        /// <summary>
        /// For SizeRunning Units
        /// </summary>
        public virtual ICollection<LoadDefaults> LoadDefaultsSizeRunning { get; set; }

        /// <summary>
        /// For SizeStarting Units
        /// </summary>
        public virtual ICollection<LoadDefaults> LoadDefaultsSizeStarting { get; set; }

        /// <summary>
        /// For Cooling Units
        /// </summary>
        public virtual ICollection<LoadDefaults> LoadDefaultsSizeCooling { get; set; }

        /// <summary>
        /// For KVA Units
        /// </summary>
        public virtual ICollection<LoadDefaults> LoadDefaultsSizeKVA { get; set; }

        public virtual ICollection<BasicLoad> BasicLoad1 { get; set; }

        public virtual ICollection<BasicLoad> BasicLoad2 { get; set; }

        public virtual ICollection<ACLoad> ACLoads { get; set; }

        public virtual ICollection<UPSLoad> UPSLoads { get; set; }
    }
}
