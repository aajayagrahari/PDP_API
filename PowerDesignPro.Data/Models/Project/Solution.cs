using PowerDesignPro.Data.Models.Loads;
using System;
using System.Collections.Generic;

namespace PowerDesignPro.Data.Models
{
    /// <summary>
    /// Contains a solution detail for Project
    /// </summary>
    /// <seealso cref="PowerDesignPro.Data.Models.BaseEntity" />
    /// <seealso cref="PowerDesignPro.Data.Models.IEntity" />
    public class Solution : BaseEntity, IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Solution"/> class.
        /// </summary>
        public Solution()
        {
            SolutionSetup = new HashSet<SolutionSetup>();
            LoadSequence = new HashSet<LoadSequence>();
            BasicLoadList = new HashSet<BasicLoad>();
            MotorLoadList = new HashSet<MotorLoad>();
            ACLoadList = new HashSet<ACLoad>();
            UPSLoadList = new HashSet<UPSLoad>();
            LightingLoadList = new HashSet<LightingLoad>();
            WelderLoadList = new HashSet<WelderLoad>();
            RecommendedProduct = new HashSet<RecommendedProduct>();
            GasPipingSolutions = new HashSet<GasPipingSolution>();
            ExhaustPipingSolutions = new HashSet<ExhaustPipingSolution>();
            SharedProjectSolution = new HashSet<SharedProjectSolution>();
            RequestsForQuote = new HashSet<RequestForQuote>();
        }

        /// <summary>
        /// Gets or sets the project identifier.
        /// </summary>
        /// <value>
        /// The project identifier.
        /// </value>
        public int ProjectID { get; set; }

        /// <summary>
        /// Gets or sets the name of the solution.
        /// </summary>
        /// <value>
        /// The name of the solution.
        /// </value>
        public string SolutionName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the spec reference number.
        /// </summary>
        /// <value>
        /// The spec reference number.
        /// </value>
        public string SpecRefNumber { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>
        /// The created date time.
        /// </value>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the modified date time.
        /// </summary>
        /// <value>
        /// The modified date time.
        /// </value>
        public DateTime ModifiedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        /// <value>
        /// The modified by.
        /// </value>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Solution"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        public bool Active { get; set; }

        public string OwnedBy { get; set; }

        public string EditAccessNotes { get; set; }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>
        /// The project.
        /// </value>
        public virtual Project Project { get; set; }

        public virtual ICollection<LoadSequence> LoadSequence { get; set; }

        /// <summary>
        /// Gets or sets the solution setup.
        /// </summary>
        /// <value>
        /// The solution setup.
        /// </value>
        public virtual ICollection<SolutionSetup> SolutionSetup { get; set; }

        public virtual ICollection<BasicLoad> BasicLoadList { get; set; }

        public virtual ICollection<MotorLoad> MotorLoadList { get; set; }

        public virtual ICollection<ACLoad> ACLoadList { get; set; }

        public virtual ICollection<LightingLoad> LightingLoadList { get; set; }

        public virtual ICollection<WelderLoad> WelderLoadList { get; set; }

        public virtual ICollection<UPSLoad> UPSLoadList { get; set; }

        public virtual ICollection<RecommendedProduct> RecommendedProduct { get; set; }

        public virtual ICollection<GasPipingSolution> GasPipingSolutions { get; set; }

        public virtual ICollection<ExhaustPipingSolution> ExhaustPipingSolutions { get; set; }

        public virtual ICollection<SharedProjectSolution> SharedProjectSolution { get; set; }

        public virtual ICollection<RequestForQuote> RequestsForQuote { get; set; }
    }
}
