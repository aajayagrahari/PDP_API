using System.Collections.Generic;

namespace PowerDesignPro.Common.UnitMeasure
{
    /// <summary>
    /// Contains a collection of all the Units
    /// </summary>
    public class UnitMeasureCollection
    {
        /// <summary>
        /// Gets the unit measures.
        /// </summary>
        /// <value>
        /// The unit measures.
        /// </value>
        public List<IUnitMeasure> UnitMeasures
        {
            get {

                return new List<IUnitMeasure>
                {
                    new English(),
                    new Metric()
                };
            }
        }
    }
}
