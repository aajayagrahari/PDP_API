using System;

namespace PowerDesignPro.Common.CustomException
{
    /// <summary>
    /// Custom Exception class to handle all the required custom exceptions.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class PowerDesignProException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PowerDesignProException"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="category">The category.</param>
        /// <param name="errorMessage">The error message.</param>
        public PowerDesignProException(string code, string category, string errorMessage = "") : base(errorMessage)
        {
            Code = code;
            Category = category;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string Category { get; set; }
    }
}
