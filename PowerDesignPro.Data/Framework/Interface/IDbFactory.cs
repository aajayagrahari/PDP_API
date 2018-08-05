using System;

namespace PowerDesignPro.Data.Framework.Interface
{
    public interface IDbFactory : IDisposable
    {
        ApplicationDbContext Get();
    }
}
