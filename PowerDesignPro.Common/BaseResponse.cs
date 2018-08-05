using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Common
{
    public class BaseResponse
    {
        public int ErrorCode { get; set; }

        public string ErrorDescription { get; set; }

    }

    public class PostResponse : BaseResponse
    {
        public string Message { get; set; }
    }
}
