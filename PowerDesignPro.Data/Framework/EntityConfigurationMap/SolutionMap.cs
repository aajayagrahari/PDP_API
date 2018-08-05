using PowerDesignPro.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PowerDesignPro.Data.Framework.EntityConfigurationMap
{
    public class SolutionMap : EntityTypeConfiguration<Solution>
    {
        public SolutionMap()
        {
            HasKey(x => x.ID).Property(x=>x.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.SolutionName).IsRequired().HasMaxLength(256).HasColumnOrder(2);
            Property(x => x.Description).HasMaxLength(512);
            Property(x => x.SpecRefNumber).HasMaxLength(128);

            ToTable("tSolution");
        }
    }
}
