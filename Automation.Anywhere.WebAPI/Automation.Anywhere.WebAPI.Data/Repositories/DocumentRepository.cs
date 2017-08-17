using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation.Anywhere.WebAPI.Data.Infrastructure;
using Automation.Anywhere.WebAPI.Domain;

namespace Automation.Anywhere.WebAPI.Data.Repositories
{
    public class DocumentRepository : RepositoryBase<Document>, IDocumentRepository
    {
        public DocumentRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public Document GetDocumentByTitle(string documentTitle)
        {
            var _document = this.DbContext.Documents.Where(b => b.Title == documentTitle).FirstOrDefault();

            return _document;
        }
    }

    public interface IDocumentRepository : IRepository<Document>
    {
        Document GetDocumentByTitle(string documentTitle);
    }
}
