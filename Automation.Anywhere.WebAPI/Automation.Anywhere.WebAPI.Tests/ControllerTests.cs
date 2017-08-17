#region Usings
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using Automation.Anywhere.WebAPI.API.Core.Controllers;
using Automation.Anywhere.WebAPI.Data;
using Automation.Anywhere.WebAPI.Data.Infrastructure;
using Automation.Anywhere.WebAPI.Data.Repositories;
using Automation.Anywhere.WebAPI.Domain;
using Automation.Anywhere.WebAPI.Service;
#endregion

namespace Automation.Anywhere.WebAPI.Tests
{
    [TestFixture]
    public class ControllerTests
    {
        #region Variables
        IDocumentService _documentService;
        IDocumentRepository _documentRepository;
        IUnitOfWork _unitOfWork;
        List<Document> _randomDocuments;
        #endregion

        #region Setup
        [SetUp]
        public void Setup()
        {
            _randomDocuments = SetupDocuments();

            _documentRepository = SetupDocumentRepository();
            _unitOfWork = new Mock<IUnitOfWork>().Object;
            _documentService = new DocumentService(_documentRepository, _unitOfWork);
        }

        public List<Document> SetupDocuments()
        {
            int _counter = new int();
            List<Document> _documents = DocumentVaultInitializer.GetAllDocuments();

            foreach (Document _document in _documents)
                _document.ID = ++_counter;

            return _documents;
        }

        public IDocumentRepository SetupDocumentRepository()
        {
            // Init repository
            var repo = new Mock<IDocumentRepository>();

            // Setup mocking behavior
            repo.Setup(r => r.GetAll()).Returns(_randomDocuments);

            repo.Setup(r => r.GetById(It.IsAny<int>()))
                .Returns(new Func<int, Document>(
                    id => _randomDocuments.Find(a => a.ID.Equals(id))));

            repo.Setup(r => r.Add(It.IsAny<Document>()))
                .Callback(new Action<Document>(newDocument =>
                {
                    dynamic maxDocumentID = _randomDocuments.Last().ID;
                    dynamic nextDocumentID = maxDocumentID + 1;
                    newDocument.ID = nextDocumentID;
                    newDocument.DateCreated = DateTime.Now;
                    _randomDocuments.Add(newDocument);
                }));

            repo.Setup(r => r.Update(It.IsAny<Document>()))
                .Callback(new Action<Document>(x =>
                {
                    var oldDocument = _randomDocuments.Find(a => a.ID == x.ID);
                    oldDocument.DateEdited = DateTime.Now;
                    oldDocument.OnlineURL = x.OnlineURL;
                    oldDocument.Title = x.Title;
                    oldDocument.Contents = x.Contents;
                    oldDocument.DocumentVaultID = x.DocumentVaultID;
                }));

            repo.Setup(r => r.Delete(It.IsAny<Document>()))
                .Callback(new Action<Document>(x =>
                {
                    var _documentToRemove = _randomDocuments.Find(a => a.ID == x.ID);

                    if (_documentToRemove != null)
                        _randomDocuments.Remove(_documentToRemove);
                }));

            // Return mock implementation
            return repo.Object;
        }

        #endregion

        #region Tests

        [Test]
        public void ControlerShouldReturnAllDocuments()
        {
            var _documentsController = new DocumentsController(_documentService);

            var result = _documentsController.GetDocuments();

            CollectionAssert.AreEqual(result, _randomDocuments);
        }

        [Test]
        public void ControlerShouldReturnLastDocument()
        {
            var _documentsController = new DocumentsController(_documentService);

            var result = _documentsController.GetDocument(3) as OkNegotiatedContentResult<Document>;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.Title, _randomDocuments.Last().Title);
        }

        [Test]
        public void ControlerShouldPutReturnBadRequestResult()
        {
            var _documentsController = new DocumentsController(_documentService)
            {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri("http://localhost/api/Documents/-1")
                }
            };

            var badresult = _documentsController.PutDocument(-1, new Document() { Title = "Unknown Document" });
            Assert.That(badresult, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        public void ControlerShouldPutUpdateFirstDocument()
        {
            var _documentsController = new DocumentsController(_documentService)
            {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri("http://localhost/api/Documents/1")
                }
            };

            IHttpActionResult updateResult = _documentsController.PutDocument(1, new Document()
            {
                ID = 1,
                Title = "ASP.NET Web API feat. OData",
                OnlineURL = "http://t.co/fuIbNoc7Zhjhsga",
                Contents = @"OData is an open standard protocol.."
            }) as IHttpActionResult;

            Assert.That(updateResult, Is.TypeOf<StatusCodeResult>());

            StatusCodeResult statusCodeResult = updateResult as StatusCodeResult;

            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            Assert.That(_randomDocuments.First().OnlineURL, Is.EqualTo("http://t.co/fuIbNoc7Zhjhsga"));
        }

        [Test]
        public void ControlerShouldPostNewDocument()
        {
            var Document = new Document
            {
                Title = "Web API Unit Testing",
                OnlineURL = "http://harshal.com/web-api-unit-testing",
                Author = "Harshal Shah",
                DateCreated = DateTime.Now,
                Contents = "Unit testing Web API.."
            };

            var _documentsController = new DocumentsController(_documentService)
            {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("http://localhost/api/Documents")
                }
            };

            _documentsController.Configuration.MapHttpAttributeRoutes();
            _documentsController.Configuration.EnsureInitialized();
            _documentsController.RequestContext.RouteData = new HttpRouteData(
            new HttpRoute(), new HttpRouteValueDictionary { { "_documentsController", "Documents" } });
            var result = _documentsController.PostDocument(Document) as CreatedAtRouteNegotiatedContentResult<Document>;

            Assert.That(result.RouteName, Is.EqualTo("DefaultApi"));
            Assert.That(result.Content.ID, Is.EqualTo(result.RouteValues["id"]));
            Assert.That(result.Content.ID, Is.EqualTo(_randomDocuments.Max(a => a.ID)));
        }

        [Test]
        public void ControlerShouldNotPostNewDocument()
        {
            var Document = new Document
            {
                Title = "Web API Unit Testing",
                OnlineURL = "http://harshal.com/web-api-unit-testing",
                Author = "Harshal Shah",
                DateCreated = DateTime.Now,
                Contents = null
            };

            var _documentsController = new DocumentsController(_documentService)
            {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("http://localhost/api/Documents")
                }
            };

            _documentsController.Configuration.MapHttpAttributeRoutes();
            _documentsController.Configuration.EnsureInitialized();
            _documentsController.RequestContext.RouteData = new HttpRouteData(
            new HttpRoute(), new HttpRouteValueDictionary { { "Controller", "Documents" } });
            _documentsController.ModelState.AddModelError("Contents", "Contents is required field");

            var result = _documentsController.PostDocument(Document) as InvalidModelStateResult;

            Assert.That(result.ModelState.Count, Is.EqualTo(1));
            Assert.That(result.ModelState.IsValid, Is.EqualTo(false));
        }

        #endregion
    }
}
