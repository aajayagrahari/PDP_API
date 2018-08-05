using PowerDesignPro.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace PowerDesignPro.Data.Framework.EntityConfigurationMap
{
    public class LoadDefaultsMap : EntityTypeConfiguration<LoadDefaults>
    {
        public LoadDefaultsMap()
        {
            HasKey(x => x.ID);

            Property(x => x.SizeRunning).HasPrecision(5, 2);
            Property(x => x.SizeStarting).HasPrecision(5, 2);            

            HasOptional(x => x.SizeRunningUnits)
                .WithMany(x => x.LoadDefaultsSizeRunning)
                .HasForeignKey(x => x.SizeRunningUnitsID);

            HasOptional(x => x.SizeStartingUnits)
                .WithMany(x => x.LoadDefaultsSizeStarting)
                .HasForeignKey(x => x.SizeStartingUnitsID);

            HasOptional(x => x.CoolingUnits)
                .WithMany(x => x.LoadDefaultsSizeCooling)
                .HasForeignKey(x => x.CoolingUnitsID);

            HasOptional(x => x.RunningPF)
               .WithMany(x => x.LoadDefaults1)
               .HasForeignKey(x => x.RunningPFID);

            HasOptional(x => x.StartingPF)
              .WithMany(x => x.LoadDefaults2)
              .HasForeignKey(x => x.StartingPFID);

            HasOptional(x => x.SizeKVAUnits)
              .WithMany(x => x.LoadDefaultsSizeKVA)
              .HasForeignKey(x => x.SizeKVAUnitsID);

            ToTable("tLoadDefaults");
        }
    }
}
