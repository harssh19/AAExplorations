using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Anywhere.WebAPI.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        DocumentVaultEntities dbContext;

        public DocumentVaultEntities Init()
        {
            return dbContext ?? (dbContext = new DocumentVaultEntities());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
