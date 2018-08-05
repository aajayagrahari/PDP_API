using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerDesignPro.Data.Models;

namespace PowerDesignPro.Data.Framework.EntityConfigurationMap
{
    class UPSLoadMap : EntityTypeConfiguration<UPSLoad>
    {
        public UPSLoadMap()
        {
            HasKey(x => x.ID);

            HasOptional(x => x.SizeKVAUnits)
                .WithMany(x => x.UPSLoads)
                .HasForeignKey(x => x.SizeKVAUnitsID);

            ToTable("tUPSLoad");
        }
    }
}