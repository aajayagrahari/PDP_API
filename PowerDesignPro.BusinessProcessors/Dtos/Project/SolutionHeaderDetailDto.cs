using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class SolutionHeaderDetailDto
    {
        public int ProjectID { get; set; }

        /// <summary>
        /// Gets or sets the solution identifier.
        /// </summary>
        /// <value>
        /// The solution identifier.
        /// </value>
        public int SolutionID { get; set; }

        /// <summary>
        /// Gets or sets the name of the solution.
        /// </summary>
        /// <value>
        /// The name of the solution.
        /// </value>
        public string SolutionName { get; set; }

        /// <summary>
        /// Gets or sets the access level for the user to the solution.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public bool IsReadOnlyAccess { get; set; }

        public bool ShowGrantAccess { get; set; }
    }
}
