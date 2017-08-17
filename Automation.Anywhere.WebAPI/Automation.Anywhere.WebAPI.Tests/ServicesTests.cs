#region Using
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation.Anywhere.WebAPI.Data;
using Automation.Anywhere.WebAPI.Data.Infrastructure;
using Automation.Anywhere.WebAPI.Data.Repositories;
using Automation.Anywhere.WebAPI.Domain;
using Automation.Anywhere.WebAPI.Service;
#endregion

namespace Automation.Anywhere.WebAPI.Tests
{
    [TestFixture]
    public class ServicesTests
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
            _randomDocuments = SetupArticles();

            _documentRepository = SetupArticleRepository();
            _unitOfWork = new Mock<IUnitOfWork>().Object;
            _documentService = new DocumentService(_documentRepository, _unitOfWork);
        }

        public List<Document> SetupArticles()
        {
            int _counter = new int();
            List<Document> _documents = DocumentVaultInitializer.GetAllDocuments();

            foreach (Document _document in _documents)
                _document.ID = ++_counter;

            return _documents;
        }

        public IDocumentRepository SetupArticleRepository()
        {
            // Init repository
            var repo = new Mock<IDocumentRepository>();

            // Setup mocking behavior
            repo.Setup(r => r.GetAll()).Returns(_randomDocuments);

            repo.Setup(r => r.GetById(It.IsAny<int>()))
                .Returns(new Func<int, Document>(
                    id => _randomDocuments.Find(a => a.ID.Equals(id))));

            repo.Setup(r => r.Add(It.IsAny<Document>()))
                .Callback(new Action<Document>(newArticle =>
                {
                    dynamic maxDocumentID = _randomDocuments.Last().ID;
                    dynamic nextDocumentID = maxDocumentID + 1;
                    newArticle.ID = nextDocumentID;
                    newArticle.DateCreated = DateTime.Now;
                    _randomDocuments.Add(newArticle);
                }));

            repo.Setup(r => r.Update(It.IsAny<Document>()))
                .Callback(new Action<Document>(x =>
                    {
                        var oldDocument = _randomDocuments.Find(a => a.ID == x.ID);
                        oldDocument.DateEdited = DateTime.Now;
                        oldDocument = x;
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
        public void ServiceShouldReturnAllArticles()
        {
            var articles = _documentService.GetDocuments();

            Assert.That(articles, Is.EqualTo(_randomDocuments));
        }

        [Test]
        public void ServiceShouldReturnRightArticle()
        {
            var wcfSecurityArticle = _documentService.GetDocument(2);

            Assert.That(wcfSecurityArticle,
                Is.EqualTo(_randomDocuments.Find(a => a.Title.Contains("Secure WCF Services"))));
        }

        [Test]
        public void ServiceShouldAddNewArticle()
        {
            var _newDocument = new Document()
            {
                Author = "Harshal Harshal",
                Contents = "..ylniatrec lliw uoy ,repoleved CVM TEN.PSA na era uoy fI",
                Title = ")smroF beW( TEN.PSA ni gnitooR LRUenilnO",
                OnlineURL = "http://harshal.com/2013/12/15/url-rooting-in-asp-net-web-forms/"
            };

            int _maxArticleIDBeforeAdd = _randomDocuments.Max(a => a.ID);
            _documentService.CreateDocument(_newDocument);

            Assert.That(_newDocument, Is.EqualTo(_randomDocuments.Last()));
            Assert.That(_maxArticleIDBeforeAdd + 1, Is.EqualTo(_randomDocuments.Last().ID));
        }

        [Test]
        public void ServiceShouldUpdateArticle()
        {
            var _firstDocument = _randomDocuments.First();

            _firstDocument.Title = "OData feat. ASP.NET Web API"; // reversed :-)
            _firstDocument.OnlineURL = "http://t.co/fuIbNoc7Zhjhsga"; // short link
            _documentService.UpdateDocument(_firstDocument);

            Assert.That(_firstDocument.DateEdited, Is.Not.EqualTo(DateTime.MinValue));
            Assert.That(_firstDocument.OnlineURL, Is.EqualTo("http://t.co/fuIbNoc7Zhjhsga"));
            Assert.That(_firstDocument.ID, Is.EqualTo(1)); // hasn't changed
        }

        [Test]
        public void ServiceShouldDeleteArticle()
        {
            int maxID = _randomDocuments.Max(a => a.ID); // Before removal
            var _lastDocument = _randomDocuments.Last();

            // Remove last article
            _documentService.DeleteDocument(_lastDocument);

            Assert.That(maxID, Is.GreaterThan(_randomDocuments.Max(a => a.ID))); // Max reduced by 1
        }

        #endregion
    }
}
