#region Using
using Microsoft.Owin.Hosting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Automation.Anywhere.WebAPI.API.Core.Filters;
using Automation.Anywhere.WebAPI.Data;
using Automation.Anywhere.WebAPI.Domain;
using Automation.Anywhere.WebAPI.Tests.Hosting;
#endregion

namespace Automation.Anywhere.WebAPI.Tests
{
    [TestFixture]
    public class ActionFilterTests
    {
        #region Variables
        private List<Document> _documents;
        #endregion

        #region Setup
        [SetUp]
        public void Setup()
        {
            _documents = DocumentVaultInitializer.GetAllDocuments();
        }
        #endregion

        #region Tests
        [Test]
        public void ShouldSortArticlesByTitle()
        {
            var filter = new DocumentsReversedFilter();
            var executedContext = new HttpActionExecutedContext(new HttpActionContext
            {
                Response = new HttpResponseMessage(),
            }, null);

            executedContext.Response.Content = new ObjectContent<List<Document>>(new List<Document>(_documents), new JsonMediaTypeFormatter());

            filter.OnActionExecuted(executedContext);

            var _returnedDocuments = executedContext.Response.Content.ReadAsAsync<List<Document>>().Result;

            Assert.That(_returnedDocuments.First(), Is.EqualTo(_documents.Last()));
        }

        [Test]
        public void ShouldCallToControllerActionReverseDocuments()
        {
            //Arrange
            var address = "http://localhost:9000/";

            using (WebApp.Start<Startup>(address))
            {
                HttpClient _client = new HttpClient();
                var response = _client.GetAsync(address + "api/documents").Result;

                var _returnedDocuments = response.Content.ReadAsAsync<List<Document>>().Result;

                Assert.That(_returnedDocuments.First().Title, Is.EqualTo(DocumentVaultInitializer.GetAllDocuments().Last().Title));
            }
        }
        #endregion
    }
}
