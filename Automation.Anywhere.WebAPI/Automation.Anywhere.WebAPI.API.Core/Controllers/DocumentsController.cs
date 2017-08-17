using System;
using System.Collections.Generic;
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
    public class DocumentsController : ApiController
    {
        private IDocumentService _documentService;

        public DocumentsController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        // GET: api/Documents
        public IEnumerable<Document> GetDocuments()
        {
            return _documentService.GetDocuments();
        }

        // GET: api/Documents/5
        [ResponseType(typeof(Document))]
        public IHttpActionResult GetDocument(int id)
        {
            Document document = _documentService.GetDocument(id);
            if (document == null)
            {
                return NotFound();
            }

            return Ok(document);
        }

        // PUT: api/Documents/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDocument(int id, Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != document.ID)
            {
                return BadRequest();
            }

            _documentService.UpdateDocument(document);

            try
            {
                _documentService.SaveDocument();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentExists(id))
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

        // POST: api/Documents
        [ResponseType(typeof(Document))]
        public IHttpActionResult PostDocument(Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _documentService.CreateDocument(document);

            return CreatedAtRoute("DefaultApi", new { id = document.ID }, document);
        }

        // DELETE: api/Documents/5
        [ResponseType(typeof(Document))]
        public IHttpActionResult DeleteDocument(int id)
        {
            Document document = _documentService.GetDocument(id);
            if (document == null)
            {
                return NotFound();
            }

            _documentService.DeleteDocument(document);

            return Ok(document);
        }

        private bool DocumentExists(int id)
        {
            return _documentService.GetDocument(id) != null;
        }
    }
}
