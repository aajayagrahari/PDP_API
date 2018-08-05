using PowerDesignPro.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace PowerDesignPro.Data.Framework.EntityConfigurationMap
{
    public class MotorCalculationMap : EntityTypeConfiguration<MotorCalculation>
    {
        public MotorCalculationMap()
        {
            HasKey(x => x.ID);

            HasOptional(x => x.StartingCodeNema)
              .WithMany(x => x.MotorCalculation1)
              .HasForeignKey(x => x.StartingCodeIDNema);

            HasOptional(x => x.StartingCodeIEC)
              .WithMany(x => x.MotorCalculation2)
              .HasForeignKey(x => x.StartingCodeIDIEC);

            ToTable("tMotorCalculation");
        }
    }
}
