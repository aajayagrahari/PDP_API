using PowerDesignPro.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace PowerDesignPro.Data.Framework.EntityConfigurationMap
{
    public class GasPipingSolutionMap : EntityTypeConfiguration<GasPipingSolution>
    {
        public GasPipingSolutionMap()
        {
            HasRequired(x => x.Units)
                .WithMany(x => x.GasPipingSolutions)
                .HasForeignKey(x => x.UnitID);

            HasRequired(x => x.Solution)
                .WithMany(x => x.GasPipingSolutions)
                .HasForeignKey(x => x.SolutionID);

            HasRequired(x => x.GasPipingSizingMethod)
                .WithMany(x => x.GasPipingSolutions)
                .HasForeignKey(x => x.SizingMethodID);

            HasRequired(x => x.GasPipingPipeSize)
                .WithMany(x => x.GasPipingSolutions)
                .HasForeignKey(x => x.PipeSizeID);

            ToTable("tGasPipingSolution");
        }
    }
}
