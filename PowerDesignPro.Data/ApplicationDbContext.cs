using Microsoft.AspNet.Identity.EntityFramework;
using PowerDesignPro.Data.Framework.Annotations;
using PowerDesignPro.Data.Framework.EntityConfigurationMap;
using PowerDesignPro.Data.Models;
using PowerDesignPro.Data.Models.User;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PowerDesignPro.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("PowerDesignProConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Solution> Solutions { get; set; }

        public DbSet<TraceMessage> TraceMessages { get; set; }

        public DbSet<HarmonicProfile> HarmonicProfiles { get; set; }

        public DbSet<RequestForQuote> RequestsForQuote { get; set; }

        #region Solution Setup Entities
        public DbSet<AmbientTemperature> AmbientTemperatures { get; set; }

        public DbSet<ContinuousAllowableVoltageDistortion> ContinuousAllowableVoltageDistortions { get; set; }

        public DbSet<DesiredRunTime> DesiredRunTimes { get; set; }

        public DbSet<DesiredSound> DesiredSounds { get; set; }

        public DbSet<Elevation> Elevations { get; set; }

        public DbSet<EnclosureType> EnclosureTypes { get; set; }

        public DbSet<EngineDuty> EngineDuty { get; set; }

        public DbSet<Frequency> Frequencies { get; set; }

        public DbSet<FrequencyDip> FrequencyDip { get; set; }

        public DbSet<FrequencyDipUnits> FrequencyDipUnits { get; set; }

        public DbSet<FuelTank> FuelTanks { get; set; }

        public DbSet<FuelType> FuelTypes { get; set; }

        public DbSet<LoadSequenceCyclic> LoadSequenceCyclics { get; set; }

        public DbSet<MaxRunningLoad> MaxRunningLoads { get; set; }

        public DbSet<MomentaryAllowableVoltageDistortion> MomentaryAllowableVoltageDistortions { get; set; }

        public DbSet<SolutionApplication> SolutionApplications { get; set; }

        public DbSet<RegulatoryFilter> RegulatoryFilter { get; set; }

        public DbSet<Units> Units { get; set; }

        public DbSet<VoltageDip> VoltageDip { get; set; }

        public DbSet<VoltageDipUnits> VoltageDipUnits { get; set; }

        public DbSet<VoltageNominal> VoltageNominals { get; set; }

        public DbSet<VoltagePhase> VoltagePhases { get; set; }

        public DbSet<VoltageSpecific> VoltageSpecifics { get; set; }

        public DbSet<SolutionSetup> SolutionSetup { get; set; }

        public DbSet<UserDefaultSolutionSetup> UserDefaultSolutionSetup { get; set; }

        #endregion


        #region Load Entities

        public DbSet<LoadDefaults> LoadDefaults { get; set; }

        public DbSet<HarmonicContent> HarmonicContents { get; set; }

        public DbSet<HarmonicDeviceType> HarmonicDeviceTypes { get; set; }

        public DbSet<LoadFamily> LoadFamilies { get; set; }

        public DbSet<PF> PF { get; set; }

        public DbSet<Sequence> Sequences { get; set; }

        public DbSet<SequenceType> SequenceTypes { get; set; }

        public DbSet<SizeUnits> SizeUnits { get; set; }

        public DbSet<StartingMethod> StartingMethod { get; set; }

        public DbSet<Compressors> Compressors { get; set; }

        public DbSet<CoolingLoad> CoolingLoad { get; set; }

        public DbSet<ReheatLoad> ReheatLoad { get; set; }

        public DbSet<LightingType> LightingType { get; set; }

        public DbSet<WelderType> WelderType { get; set; }

        public DbSet<Phase> Phase { get; set; }

        public DbSet<Efficiency> Efficiency { get; set; }

        public DbSet<ChargeRate> ChargeRate { get; set; }

        public DbSet<PowerFactor> PowerFactor { get; set; }

        public DbSet<LoadLevel> LoadLevel { get; set; }

        public DbSet<UPSType> UPSType { get; set; }

        public DbSet<MotorLoadLevel> MotorLoadLevel { get; set; }

        public DbSet<MotorLoadType> MotorLoadType { get; set; }

        public DbSet<MotorCalculation> MotorCalculation { get; set; }

        public DbSet<MotorType> MotorType { get; set; }

        public DbSet<StartingCode> StartingCode { get; set; }

        public DbSet<ConfigurationInput> ConfigurationInput { get; set; }
        #endregion


        #region Account Entities

        public DbSet<State> States { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<UserCategory> UserCategories { get; set; }

        public DbSet<Brand> Brands { get; set; }

        #endregion


        #region Solution Summary Entities

        public DbSet<FamilySelectionMethod> FamilySelectionMethods { get; set; }

        public DbSet<ProductFamily> ProductFamilies { get; set; }

        //public DbSet<ModuleSize> ModuleSizes { get; set; }

        public DbSet<ParallelQuantity> ParallelQuantites { get; set; }

        public DbSet<SizingMethod> SizingMethods { get; set; }

        public DbSet<Generator> Generators { get; set; }

        public DbSet<GeneratorAvailableVoltage> GeneratorAvailableVoltages { get; set; }

        public DbSet<GeneratorAvailableAlternator> GeneratorAvailableAlternators { get; set; }

        public DbSet<GeneratorRegulatoryFilter> GeneratorRegulatoryFilters { get; set; }

        public DbSet<AlternatorFamily> AlternatorFamilies { get; set; }

        public DbSet<Alternator> Alternators { get; set; }

        public DbSet<Documentation> Documentation { get; set; }

        public DbSet<ExhaustPipingSolution> ExhaustPipingSolution { get; set; }

        public DbSet<ExhaustPipingPipeSize> ExhaustPipingPipeSize { get; set; }

        public DbSet<GasPipingSolution> GasPipingSolution { get; set; }

        public DbSet<GasPipingSizingMethod> GasPipingSizingMethod { get; set; }

        public DbSet<GasPipingPipeSize> GasPipingPipeSize { get; set; }

        public DbSet<ExhaustSystemConfiguration> ExhaustSystemConfigurations { get; set; }

        public DbSet<SearchFilter> SearchFilter { get; set; }
        #endregion


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            Precision.ConfigureModelBuilder(modelBuilder);

            modelBuilder.Configurations.Add(new ProjectMap());

            modelBuilder.Configurations.Add(new VoltageNominalMap());

            modelBuilder.Configurations.Add(new VoltageSpecificMap());

            modelBuilder.Configurations.Add(new SolutionMap());

            modelBuilder.Configurations.Add(new SolutionSetupMap());

            modelBuilder.Configurations.Add(new UserDefaultSolutionSetupMap());

            modelBuilder.Configurations.Add(new LoadDefaultsMap());

            modelBuilder.Configurations.Add(new BasicLoadMap());

            modelBuilder.Configurations.Add(new MotorCalculationMap());

            modelBuilder.Configurations.Add(new ACLoadMap());

            modelBuilder.Configurations.Add(new UPSLoadMap());

            modelBuilder.Configurations.Add(new GasPipingSolutionMap());

            modelBuilder.Configurations.Add(new ExhaustPipingSolutionMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}