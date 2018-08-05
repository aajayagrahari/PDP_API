using System;
using PowerDesignPro.Data;
using PowerDesignPro.BusinessProcessors.Interface;
using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.Data.Framework.Interface;
using PowerDesignPro.Data.Models;

namespace PowerDesignPro.BusinessProcessors
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="PowerDesignPro.BusinessProcessors.Interface.IUser" />
    public class UserRegisterProcessor : IUser
    {
        /// <summary>
        /// Registers the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object Register(UserRegisterDto model)
        {
            throw new NotImplementedException();
        }
    }
}
