using PowerDesignPro.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace PowerDesignPro.Data.Framework.EntityConfigurationMap
{
    public class BasicLoadMap : EntityTypeConfiguration<BasicLoad>
    {
        public BasicLoadMap()
        {
            HasKey(x => x.ID);

            Property(x => x.SizeRunning).HasPrecision(10, 2);
            Property(x => x.SizeStarting).HasPrecision(10, 2);

            HasOptional(x => x.SizeRunningUnits)
                .WithMany(x => x.BasicLoad1)
                .HasForeignKey(x => x.SizeRunningUnitsID);

            HasOptional(x => x.SizeStartingUnits)
                .WithMany(x => x.BasicLoad2)
                .HasForeignKey(x => x.SizeStartingUnitsID);

            HasOptional(x => x.RunningPF)
               .WithMany(x => x.BasicLoad1)
               .HasForeignKey(x => x.RunningPFID);

            HasOptional(x => x.StartingPF)
              .WithMany(x => x.BasicLoad2)
              .HasForeignKey(x => x.StartingPFID);

            ToTable("tBasicLoad");
        }
    }
}
