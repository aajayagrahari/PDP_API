using PowerDesignPro.Data.Framework.Interface;

namespace PowerDesignPro.Data.Framework.DbFactory
{
    public class DbFactory : Disposable, IDbFactory
    {
        private ApplicationDbContext _dataContext;
        public ApplicationDbContext Get()
        {
            return _dataContext ?? (_dataContext = ApplicationDbContext.Create());
        }
        protected override void DisposeCore()
        {
            if (_dataContext != null)
                _dataContext.Dispose();
        }
    }
}
