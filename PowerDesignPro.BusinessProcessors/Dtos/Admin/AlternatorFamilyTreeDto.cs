using System.Collections.Generic;

namespace PowerDesignPro.BusinessProcessors.Dtos.Admin
{
    public class AlternatorFamilyTreeDto : TreeBaseDto
    {
        public AlternatorFamilyTreeDto()
        {
            FrequencyTree = new List<FrequencyTreeDto>();
            VoltageNominalList = new List<int>();
        }

        public int Frequency { get; set; }

        public List<int> VoltageNominalList { get; set; }

        public List<FrequencyTreeDto> FrequencyTree { get; set; }
    }

    public class FrequencyTreeDto : TreeBaseDto
    {
        public FrequencyTreeDto()
        {
            VoltageNominalTree = new List<VoltageNominalTreeDto>();
        }
        public List<VoltageNominalTreeDto> VoltageNominalTree { get; set; }
    }

    public class VoltageNominalTreeDto : TreeBaseDto
    {
        public VoltageNominalTreeDto()
        {
            VoltagePhaseTree = new List<VoltagePhaseTreeDto>();
        }

        public List<VoltagePhaseTreeDto> VoltagePhaseTree { get; set; }
    }

    public class VoltagePhaseTreeDto : TreeBaseDto
    {
        public VoltagePhaseTreeDto()
        {
            AlternatorTree = new List<AlternatorTreeDto>();
        }
        public List<AlternatorTreeDto> AlternatorTree { get; set; }
    }

    public class AlternatorTreeDto : TreeBaseDto
    {

    }

    public class TreeBaseDto
    {
        public int ID { get; set; }

        public string Description { get; set; }
    }
}
