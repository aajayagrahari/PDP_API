using System.Configuration;

namespace PowerDesignPro.BusinessProcessors.Dtos.Project
{
    public class FuelConfigDto
    {
        public string Type { get; set; }

        public double SP_GR { get; set; }

        public double Viscosity { get; set; }
    }
    public class FuelConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("Type", IsKey = true, IsRequired = true)]
        public string Type
        {
            get { return (string)base["Type"]; }
            set { base["Type"] = value; }
        }

        [ConfigurationProperty("SP_GR", IsRequired = true)]
        public string SP_GR
        {
            get { return (string)base["SP_GR"]; }
            set { base["SP_GR"] = value; }
        }

        [ConfigurationProperty("Viscosity", IsRequired = true)]
        public string Viscosity
        {
            get { return (string)base["Viscosity"]; }
            set { base["Viscosity"] = value; }
        }
    }

    public class FuelConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public FuelConfigDtoCollection Instances
        {
            get { return (FuelConfigDtoCollection)this[""]; }
            set { this[""] = value; }
        }
    }

    public class FuelConfigDtoCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FuelConfigElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FuelConfigElement)element).Type;
        }
    }

}
