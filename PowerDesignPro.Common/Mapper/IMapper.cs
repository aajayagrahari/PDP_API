namespace PowerDesignPro.Common.Mapper
{
    /// <summary>
    /// Provide a generic signature to map data between two entities.
    /// </summary>
    /// <typeparam name="From">The type of the rom.</typeparam>
    /// <typeparam name="To">The type of the o.</typeparam>
    public interface IMapper<From, To>
        where From : class
        where To : class, new()
    {
        /// <summary>
        /// Adds the map.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        To AddMap(From input, string userId = null, string userName = null);

        /// <summary>
        /// Updates the map.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        void UpdateMap(From input, To output, string userId = null, string userName = null);
    }
}
