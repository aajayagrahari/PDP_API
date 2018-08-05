using PowerDesignPro.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Data.Framework.EntityConfigurationMap
{
    public class ACLoadMap: EntityTypeConfiguration<ACLoad>
    {
        public ACLoadMap()
        {
            HasKey(x => x.ID);

            HasOptional(x => x.CoolingUnits)
                .WithMany(x => x.ACLoads)
                .HasForeignKey(x => x.CoolingUnitsID);

            ToTable("tACLoad");
        }
    }
}
