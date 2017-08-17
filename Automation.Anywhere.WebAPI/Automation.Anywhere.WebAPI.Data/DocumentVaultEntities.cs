using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation.Anywhere.WebAPI.Data.Configurations;
using Automation.Anywhere.WebAPI.Domain;

namespace Automation.Anywhere.WebAPI.Data
{
    public class DocumentVaultEntities : DbContext
    {
        public DocumentVaultEntities()
            : base("DocumentVaultEntities")
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<DocumentVault> DocumentVaults { get; set; }
        public DbSet<Document> Documents { get; set; }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DocumentConfiguration());
            modelBuilder.Configurations.Add(new DocumentVaultConfiguration());
        }
    }
}
