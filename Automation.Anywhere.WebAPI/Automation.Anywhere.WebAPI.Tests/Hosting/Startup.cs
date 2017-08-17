#region Using
using Autofac;
using Autofac.Integration.WebApi;
using Moq;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;
using Automation.Anywhere.WebAPI.API.Core;
using Automation.Anywhere.WebAPI.API.Core.Controllers;
using Automation.Anywhere.WebAPI.API.Core.Filters;
using Automation.Anywhere.WebAPI.API.Core.MessageHandlers;
using Automation.Anywhere.WebAPI.Data;
using Automation.Anywhere.WebAPI.Data.Infrastructure;
using Automation.Anywhere.WebAPI.Data.Repositories;
using Automation.Anywhere.WebAPI.Domain;
using Automation.Anywhere.WebAPI.Service;
#endregion

namespace Automation.Anywhere.WebAPI.Tests.Hosting
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.MessageHandlers.Add(new HeaderAppenderHandler());
            config.MessageHandlers.Add(new EndRequestHandler());
            config.Filters.Add(new DocumentsReversedFilter());
            config.Services.Replace(typeof(IAssembliesResolver), new CustomAssembliesResolver());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.MapHttpAttributeRoutes();

            // Autofac configuration
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(typeof(DocumentsController).Assembly);

            // Unit of Work
            var _unitOfWork = new Mock<IUnitOfWork>();
            builder.RegisterInstance(_unitOfWork.Object).As<IUnitOfWork>();

            //Repositories
            var _documentsRepository = new Mock<IDocumentRepository>();
            _documentsRepository.Setup(x => x.GetAll()).Returns(
                    DocumentVaultInitializer.GetAllDocuments()
                );
            builder.RegisterInstance(_documentsRepository.Object).As<IDocumentRepository>();

            var _documentVaultRepository = new Mock<IDocumentVaultRepository>();
            _documentVaultRepository.Setup(x => x.GetAll()).Returns(
                DocumentVaultInitializer.GetDocumentVaults
                );
            builder.RegisterInstance(_documentVaultRepository.Object).As<IDocumentVaultRepository>();

            // Services
            builder.RegisterAssemblyTypes(typeof(DocumentService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterInstance(new DocumentService(_documentsRepository.Object, _unitOfWork.Object));
            builder.RegisterInstance(new DocumentVaultService(_documentVaultRepository.Object, _unitOfWork.Object));

            IContainer container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            appBuilder.UseWebApi(config);
        }
    }
}
