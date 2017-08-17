using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Automation.Anywhere.WebAPI.API.Core.MediaTypeFormatters;
using Automation.Anywhere.WebAPI.Data;
using Automation.Anywhere.WebAPI.Domain;

namespace Automation.Anywhere.WebAPI.Tests
{
    [TestFixture]
    public class MediaTypeFormatterTests
    {
        #region Variables
        DocumentVault _documentVault;
        Document _document;
        DocumentFormatter _formatter;
        #endregion

        #region Setup
        [SetUp]
        public void Setup()
        {
            _documentVault = DocumentVaultInitializer.GetDocumentVaults().First();
            _document = DocumentVaultInitializer.GetType2Documents().First();
            _formatter = new DocumentFormatter();
        }
        #endregion

        #region Tests
        [Test]
        public void FormatterShouldThrowExceptionWhenUnsupportedType()
        {
            Assert.Throws<InvalidOperationException>(() => new ObjectContent<DocumentVault>(_documentVault, _formatter));
        }

        [Test]
        public void FormatterShouldNotThrowExceptionWhenArticle()
        {
            Assert.DoesNotThrow(() => new ObjectContent<Document>(_document, _formatter));
        }

        [Test]
        public void FormatterShouldHeaderBeSetCorrectly()
        {
            var content = new ObjectContent<Document>(_document, new DocumentFormatter());

            Assert.That(content.Headers.ContentType.MediaType, Is.EqualTo("application/article"));
        }

        [Test]
        public async void FormatterShouldBeAbleToDeserializeArticle()
        {
            var content = new ObjectContent<Document>(_document, _formatter);

            var deserializedItem = await content.ReadAsAsync<Document>(new[] { _formatter });

            Assert.That(_document, Is.SameAs(deserializedItem));
        }

        [Test]
        public void FormatterShouldNotBeAbleToWriteUnsupportedType()
        {
            var canWriteDocumentVault = _formatter.CanWriteType(typeof(DocumentVault));
            Assert.That(canWriteDocumentVault, Is.False);
        }

        [Test]
        public void FormatterShouldBeAbleToWriteDocument()
        {
            var canWriteDocument = _formatter.CanWriteType(typeof(Document));
            Assert.That(canWriteDocument, Is.True);
        }
        #endregion
    }
}
