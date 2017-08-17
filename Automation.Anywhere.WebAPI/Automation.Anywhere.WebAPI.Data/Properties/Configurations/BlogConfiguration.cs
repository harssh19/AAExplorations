using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation.Anywhere.WebAPI.Domain;

namespace Automation.Anywhere.WebAPI.Data.Configurations
{
    public class DocumentVaultConfiguration : EntityTypeConfiguration<DocumentVault>
    {
        public DocumentVaultConfiguration()
        {
            ToTable("DocumentVault");
            Property(b => b.Name).IsRequired().HasMaxLength(100);
            Property(b => b.URL).IsRequired().HasMaxLength(200);
            Property(b => b.Owner).IsRequired().HasMaxLength(50);
            Property(b => b.DateCreated).HasColumnType("datetime2");
        }
    }
}
