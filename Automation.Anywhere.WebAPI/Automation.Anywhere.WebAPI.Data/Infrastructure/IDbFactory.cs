using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Anywhere.WebAPI.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        DocumentVaultEntities Init();
    }
}
