using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation.Anywhere.WebAPI.Domain;

namespace Automation.Anywhere.WebAPI.Data.Configurations
{
    public class DocumentConfiguration : EntityTypeConfiguration<Document>
    {
        public DocumentConfiguration()
        {
            ToTable("Document");
            Property(a => a.Title).IsRequired().HasMaxLength(100);
            Property(a => a.Contents).IsRequired();
            Property(a => a.Author).IsRequired().HasMaxLength(50);
            Property(a => a.OnlineURL).IsRequired().HasMaxLength(200);
            Property(a => a.DateCreated).HasColumnType("datetime2");
            Property(a => a.DateEdited).HasColumnType("datetime2");
        }
    }
}
