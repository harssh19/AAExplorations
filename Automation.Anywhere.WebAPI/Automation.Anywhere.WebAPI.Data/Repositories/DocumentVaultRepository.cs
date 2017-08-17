using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation.Anywhere.WebAPI.Data.Infrastructure;
using Automation.Anywhere.WebAPI.Domain;

namespace Automation.Anywhere.WebAPI.Data.Repositories
{
    public class DocumentVaultRepository : RepositoryBase<DocumentVault>, IDocumentVaultRepository
    {
        public DocumentVaultRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public DocumentVault GetDocumentVaultByName(string vaultName)
        {
            var _vault = this.DbContext.DocumentVaults.Where(b => b.Name == vaultName).FirstOrDefault();

            return _vault;
        }
    }

    public interface IDocumentVaultRepository : IRepository<DocumentVault>
    {
        DocumentVault GetDocumentVaultByName(string vaultName);
    }
}
