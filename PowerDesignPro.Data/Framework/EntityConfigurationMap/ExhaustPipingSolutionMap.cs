using PowerDesignPro.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace PowerDesignPro.Data.Framework.EntityConfigurationMap
{
    public class ExhaustPipingSolutionMap : EntityTypeConfiguration<ExhaustPipingSolution>
    {
        public ExhaustPipingSolutionMap()
        {
            HasRequired(x => x.Units)
                .WithMany(x => x.ExhaustPipingSolutions)
                .HasForeignKey(x => x.UnitID);

            HasRequired(x => x.Solution)
                .WithMany(x => x.ExhaustPipingSolutions)
                .HasForeignKey(x => x.SolutionID);

            HasRequired(x => x.SizingMethod)
                .WithMany(x => x.ExhaustPipingSolutions)
                .HasForeignKey(x => x.SizingMethodID);

            HasRequired(x => x.ExhaustPipingPipeSize)
                .WithMany(x => x.ExhaustPipingSolutions)
                .HasForeignKey(x => x.PipeSizeID);

            HasRequired(x => x.ExhaustSystemConfiguration)
                .WithMany(x => x.ExhaustPipingSolutions)
                .HasForeignKey(x => x.ExhaustSystemConfigurationID);

            ToTable("tExhaustPipingSolution");
        }
    }
}
