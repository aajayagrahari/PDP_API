using PowerDesignPro.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace PowerDesignPro.Data.Framework.EntityConfigurationMap
{
    public class ProjectMap : EntityTypeConfiguration<Project>
    {
        public ProjectMap()
        {
            HasKey(x => x.ID);
            Property(x => x.ProjectName).IsRequired().HasMaxLength(255);
            Property(x => x.CreatedBy).IsRequired().HasMaxLength(128);
            Property(x => x.ModifiedBy).IsRequired().HasMaxLength(128);
            Property(x => x.CreatedDateTime).IsRequired();
            Property(x => x.ModifiedDateTime).IsRequired();
            Property(x => x.Active).IsRequired();
            Property(x => x.ContactName).HasMaxLength(255);
            Property(x => x.ContactEmail).HasMaxLength(255);
            //HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            HasRequired(x => x.User)
                .WithMany(x => x.Projects)                
                .HasForeignKey(x => x.UserID)
                .WillCascadeOnDelete(false);

            ToTable("tProject");
        }
    }
}
