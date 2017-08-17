#region Using
using Microsoft.Owin.Hosting;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Automation.Anywhere.WebAPI.API.Core.Controllers;
using Automation.Anywhere.WebAPI.API.Core.MessageHandlers;
using Automation.Anywhere.WebAPI.Data;
using Automation.Anywhere.WebAPI.Data.Infrastructure;
using Automation.Anywhere.WebAPI.Data.Repositories;
using Automation.Anywhere.WebAPI.Domain;
using Automation.Anywhere.WebAPI.Service;
using Automation.Anywhere.WebAPI.Tests.Hosting;
#endregion

namespace Automation.Anywhere.WebAPI.Tests
{
    [TestFixture]
    public class MessageHandlerTests
    {
        #region Variables
        private EndRequestHandler _endRequestHandler;
        private HeaderAppenderHandler _headerAppenderHandler;
        #endregion

        #region Setup
        [SetUp]
        public void Setup()
        {
            // Direct MessageHandler test
            _endRequestHandler = new EndRequestHandler();
            _headerAppenderHandler = new HeaderAppenderHandler()
            {
                InnerHandler = _endRequestHandler
            };
        }
        #endregion

        #region Tests
        [Test]
        public async void ShouldAppendCustomHeader()
        {
            var invoker = new HttpMessageInvoker(_headerAppenderHandler);
            var result = await invoker.SendAsync(new HttpRequestMessage(HttpMethod.Get, 
                new Uri("http://localhost/api/test/")), CancellationToken.None);

            Assert.That(result.Headers.Contains("X-WebAPI-Header"), Is.True);
            Assert.That(result.Content.ReadAsStringAsync().Result, 
                Is.EqualTo("Unit testing message handlers!"));
        }

        [Test]
        public void ShouldCallToControllerActionAppendCustomHeader()
        {
            //Arrange
            var address = "http://localhost:9000/";

            using (WebApp.Start<Startup>(address))
            {
                HttpClient _client = new HttpClient();
                var response = _client.GetAsync(address + "api/documents").Result;

                Assert.That(response.Headers.Contains("X-WebAPI-Header"), Is.True);

                var _returnedDocument = response.Content.ReadAsAsync<List<Document>>().Result;
                Assert.That(_returnedDocument.Count, Is.EqualTo( DocumentVaultInitializer.GetAllDocuments().Count));
            }
        }
        #endregion
    }
}
