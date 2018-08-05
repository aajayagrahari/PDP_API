using PowerDesignPro.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PowerDesignPro.Data.Framework.EntityConfigurationMap
{
    public class SolutionSetupMap : EntityTypeConfiguration<SolutionSetup>
    {
        public SolutionSetupMap()
        {
            HasKey(x => x.ID).Property(x => x.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnOrder(0);

            HasRequired(x => x.Solution)
                .WithMany(x => x.SolutionSetup)
                .HasForeignKey(x => x.SolutionID)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.LoadSequenceCyclic1)
                .WithMany(x => x.SolutionSetups1)
                .HasForeignKey(x => x.LoadSequenceCyclic1ID)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.LoadSequenceCyclic2)
                .WithMany(x => x.SolutionSetups2)
                .HasForeignKey(x => x.LoadSequenceCyclic2ID)
                .WillCascadeOnDelete(false);

            ToTable("tSolutionSetup");
        }
    }
}
