using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Automation.Anywhere.WebAPI.Domain;
using Automation.Anywhere.WebAPI.Service;

namespace Automation.Anywhere.WebAPI.API.Core.Controllers
{
    public class DocumentVaultsController : ApiController
    {
        private IDocumentVaultService _documentVaultService;

        public DocumentVaultsController(IDocumentVaultService documentVaultService)
        {
            _documentVaultService = documentVaultService;
        }

        // GET: api/DocumentVaults
        public IEnumerable<DocumentVault> GetDocumentVaults()
        {
            return _documentVaultService.GetDocumentVaults();
        }

        // GET: api/DocumentVaults/5
        [ResponseType(typeof(DocumentVault))]
        public IHttpActionResult GetDocumentVault(int id)
        {
            DocumentVault documentvault = _documentVaultService.GetDocumentVault(id);
            if (documentvault == null)
            {
                return NotFound();
            }

            return Ok(documentvault);
        }

        // PUT: api/DocumentVaults/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBlog(int id, DocumentVault documentvault)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != documentvault.ID)
            {
                return BadRequest();
            }

            _documentVaultService.UpdateDocumentVault(documentvault);

            try
            {
                _documentVaultService.SaveDocumentVault();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentVaultExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/DocumentVaults
        [ResponseType(typeof(DocumentVault))]
        public IHttpActionResult PostDocumentVault(DocumentVault documentvault)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _documentVaultService.CreateDocumentVault(documentvault);

            return CreatedAtRoute("DefaultApi", new { id = documentvault.ID }, documentvault);
        }

        // DELETE: api/DocumentVaults/5
        [ResponseType(typeof(DocumentVault))]
        public IHttpActionResult DeleteDocumentVault(int id)
        {
            DocumentVault documentvault = _documentVaultService.GetDocumentVault(id);
            if (documentvault == null)
            {
                return NotFound();
            }

            _documentVaultService.DeleteDocumentVault(documentvault);

            return Ok(documentvault);
        }

        private bool DocumentVaultExists(int id)
        {
            return _documentVaultService.GetDocumentVault(id) != null;
        }
    }
}
