using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation.Anywhere.WebAPI.Data.Infrastructure;
using Automation.Anywhere.WebAPI.Data.Repositories;
using Automation.Anywhere.WebAPI.Domain;

namespace Automation.Anywhere.WebAPI.Service
{
    // operations you want to expose
    public interface IDocumentService
    {
        IEnumerable<Document> GetDocuments(string name = null);
        Document GetDocument(int id);
        Document GetDocument(string name);
        void CreateDocument(Document Document);
        void UpdateDocument(Document Document);
        void DeleteDocument(Document Document);
        void SaveDocument();
    }

    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository documentsRepository;
        private readonly IUnitOfWork unitOfWork;

        public DocumentService(IDocumentRepository documentsRepository, IUnitOfWork unitOfWork)
        {
            this.documentsRepository = documentsRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IDocumentService Members

        public IEnumerable<Document> GetDocuments(string title = null)
        {
            if (string.IsNullOrEmpty(title))
                return documentsRepository.GetAll();
            else
                return documentsRepository.GetAll().Where(c => c.Title.ToLower().Contains(title.ToLower()));
        }

        public Document GetDocument(int id)
        {
            var document = documentsRepository.GetById(id);
            return document;
        }

        public Document GetDocument(string title)
        {
            var document = documentsRepository.GetDocumentByTitle(title);
            return document;
        }

        public void CreateDocument(Document document)
        {
            documentsRepository.Add(document);
        }

        public void UpdateDocument(Document document)
        {
            documentsRepository.Update(document);
        }

        public void DeleteDocument(Document document)
        {
            documentsRepository.Delete(document);
        }

        public void SaveDocument()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
