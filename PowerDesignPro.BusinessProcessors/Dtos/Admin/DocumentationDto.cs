using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerDesignPro.BusinessProcessors.Dtos.Admin;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class DocumentationDto
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int Ordinal { get; set; }

        public int GeneratorID { get; set; }

        public string DocumentURL { get; set; }

        public string UserName { get; set; }
    }
}
