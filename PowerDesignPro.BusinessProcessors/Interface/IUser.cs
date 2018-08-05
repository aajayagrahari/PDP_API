using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerDesignPro.Data;
using PowerDesignPro.BusinessProcessors.Dtos;

namespace PowerDesignPro.BusinessProcessors.Interface
{
    public interface IUser
    {
        Object Register(UserRegisterDto model);

    }
}
