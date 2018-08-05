using PowerDesignPro.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace PowerDesignPro.Data.Framework.EntityConfigurationMap
{
    public class VoltageNominalMap : EntityTypeConfiguration<VoltageNominal>
    {
        public VoltageNominalMap()
        {
            HasKey(x => x.ID);
            Property(x => x.Description).IsRequired().HasMaxLength(50);
            Property(x => x.Value).IsRequired();

            HasRequired(x => x.VoltagePhase)
                .WithMany(x => x.VoltageNominals)
                .HasForeignKey(x => x.VoltagePhaseID)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Frequency)
                .WithMany(x => x.VoltageNominals)
                .HasForeignKey(x => x.FrequencyID)
                .WillCascadeOnDelete(false);

            ToTable("tVoltageNominal");
        }
    }

    public class VoltageSpecificMap : EntityTypeConfiguration<VoltageSpecific>
    {
        public VoltageSpecificMap()
        {
            HasKey(x => x.ID);
            Property(x => x.Description).IsRequired().HasMaxLength(50);
            Property(x => x.Value).IsRequired();

            HasRequired(x => x.VoltageNominal)
                .WithMany(x => x.VoltageSpecifics)
                .HasForeignKey(x => x.VoltageNominalID)
                .WillCascadeOnDelete(false);

            //HasRequired(x => x.Frequency)
            //    .WithMany(x => x.VoltageSpecific)
            //    .HasForeignKey(x => x.FrequencyID)
            //    .WillCascadeOnDelete(false);

            ToTable("tVoltageSpecific");
        }
    }
}
