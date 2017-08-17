using Automation.Anywhere.WebAPI.Data.Infrastructure;
using Automation.Anywhere.WebAPI.Data.Repositories;
using Automation.Anywhere.WebAPI.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Automation.Anywhere.WebAPI.Service
{
    // operations you want to expose
    public interface IDocumentVaultService
    {
        IEnumerable<DocumentVault> GetDocumentVaults(string name = null);
        DocumentVault GetDocumentVault(int id);
        DocumentVault GetDocumentVault(string name);
        void CreateDocumentVault(DocumentVault documentvault);
        void UpdateDocumentVault(DocumentVault documentvault);
        void SaveDocumentVault();
        void DeleteDocumentVault(DocumentVault documentvault);
    }

    public class DocumentVaultService : IDocumentVaultService
    {
        private readonly IDocumentVaultRepository documentVaultRepository;
        private readonly IUnitOfWork unitOfWork;

        public DocumentVaultService(IDocumentVaultRepository documentVaultsRepository, IUnitOfWork unitOfWork)
        {
            this.documentVaultRepository = documentVaultsRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IDocumentVaultService Members

        public IEnumerable<DocumentVault> GetDocumentVaults(string name = null)
        {
            if (string.IsNullOrEmpty(name))
                return documentVaultRepository.GetAll();
            else
                return documentVaultRepository.GetAll().Where(c => c.Name == name);
        }

        public DocumentVault GetDocumentVault(int id)
        {
            var vault = documentVaultRepository.GetById(id);
            return vault;
        }

        public DocumentVault GetDocumentVault(string name)
        {
            var vault = documentVaultRepository.GetDocumentVaultByName(name);
            return vault;
        }

        public void CreateDocumentVault(DocumentVault documentvault)
        {
            documentVaultRepository.Add(documentvault);
        }

        public void UpdateDocumentVault(DocumentVault documentvault)
        {
            documentVaultRepository.Update(documentvault);
        }

        public void DeleteDocumentVault(DocumentVault documentvault)
        {
            documentVaultRepository.Delete(documentvault);
        }

        public void SaveDocumentVault()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
