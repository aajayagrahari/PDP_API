using System.Threading.Tasks;

namespace PowerDesignPro.Common.Mapper
{
    /// <summary>
    /// Implements IMapper and provide a functionality to map data between two different objects
    /// </summary>
    /// <typeparam name="From">The type of the rom.</typeparam>
    /// <typeparam name="To">The type of the o.</typeparam>
    /// <seealso cref="PowerDesignPro.Common.Mapper.IMapper{From, To}" />
    public class AutoMapper<From, To> : IMapper<From, To>
        where From : class
        where To : class, new()
    {
        /// <summary>
        /// Adds the map. Return a new Object of output type
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public virtual To AddMap(From input, string userId = null, string userName = null)
        {
            if (input != null)
            {
                var output = new To();
                Parallel.ForEach(input.GetType().GetProperties(), (currentProp) =>
                {
                    var currentPropertyName = currentProp.Name;
                    var outProperty = output.GetType().GetProperty(currentPropertyName);

                    if (outProperty != null)
                    {
                        var inputProperty = input.GetType().GetProperty(currentPropertyName);
                        if (outProperty.PropertyType.IsEquivalentTo(inputProperty.PropertyType))
                            outProperty.SetValue(output, currentProp.GetValue(input, null));
                    }
                });

                return output;
            }

            return null;
        }

        /// <summary>
        /// Updates the map. Map data from input objects to output
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        public virtual void UpdateMap(From input, To output, string userId = null, string userName = null)
        {
            if (input != null)
            {
                Parallel.ForEach(input.GetType().GetProperties(), (currentProp) =>
                {
                    var currentPropertyName = currentProp.Name;
                    var outProperty = output.GetType().GetProperty(currentPropertyName);
                    if (outProperty != null)
                    {
                        var inputProperty = input.GetType().GetProperty(currentPropertyName);
                        if (outProperty.PropertyType.IsEquivalentTo(inputProperty.PropertyType))
                            outProperty.SetValue(output, currentProp.GetValue(input, null));
                    }
                });
            }
        }
    }
}