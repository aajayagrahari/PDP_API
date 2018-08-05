using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Interface
{
    public interface IGeneratorAlternator
    {
        GeneratorDetail GetGeneratorAlternatorDetailByID(int generatorID, SolutionSetup solutionSetup, int quantity);

        AlternatorDetail GetAlternatorDetail(Alternator alternator);
    }
}
