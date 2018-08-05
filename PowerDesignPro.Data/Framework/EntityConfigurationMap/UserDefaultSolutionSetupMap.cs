using PowerDesignPro.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace PowerDesignPro.Data.Framework.EntityConfigurationMap
{
    public class UserDefaultSolutionSetupMap : EntityTypeConfiguration<UserDefaultSolutionSetup>
    {
        public UserDefaultSolutionSetupMap()
        {
            HasKey(x => x.ID).Property(x => x.ID).HasColumnOrder(0);

            HasOptional(x => x.User)
                .WithMany(x => x.UserDefaultSolutionSetups)
                .HasForeignKey(x => x.UserID)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.LoadSequenceCyclic1)
                .WithMany(x => x.UserDefaultSolutionSetups1)
                .HasForeignKey(x => x.LoadSequenceCyclic1ID)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.LoadSequenceCyclic2)
                .WithMany(x => x.UserDefaultSolutionSetups2)
                .HasForeignKey(x => x.LoadSequenceCyclic2ID)
                .WillCascadeOnDelete(false);

            ToTable("tUserDefaultSolutionSetup");
        }
    }
}
