using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation.Anywhere.WebAPI.Domain;

namespace Automation.Anywhere.WebAPI.Data
{
    public class DocumentVaultInitializer : DropCreateDatabaseIfModelChanges<DocumentVaultEntities>
    {
        protected override void Seed(DocumentVaultEntities context)
        {
            GetDocumentVaults().ForEach(b => context.DocumentVaults.Add(b));

            context.Commit();
        }

        public static List<DocumentVault> GetDocumentVaults()
        {
            List<DocumentVault> _vault = new List<DocumentVault>();

            // Add two DocumentVaults
            DocumentVault _vault1 = new DocumentVault()
            {
                Name = "harshal's Vault",
                URL = "http://harshal.com/",
                Owner = "Harshal Shah",
                Documents = GetType2Documents()
            };

            DocumentVault _vault2 = new DocumentVault()
            {
                Name = "HarshalCodeGeeks",
                URL = "Harshalcodegeeks",
                Owner = ".NET Code Geeks",
                Documents = GetType1Documents()
            };

            _vault.Add(_vault1);
            _vault.Add(_vault2);

            return _vault;
        }

        public static List<Document> GetType2Documents()
        {
            List<Document> _documents = new List<Document>();

            Document _oData = new Document()
            {
                Author = "Harshal Shah",
                Title = "ASP.NET Web API feat. OData",
                OnlineURL = "http://harshal.com/2015/04/04/asp-net-web-api-feat-odata/",
                Contents = @"OData is an open standard protocol allowing the creation and consumption of queryable 
                            and interoperable RESTful APIs. It was initiated by Microsoft and it’s mostly known to
                            .NET Developers from WCF Data Services. There are many other server platforms supporting
                            OData services such as Node.js, PHP, Java and SQL Server Reporting Services. More over, 
                            Web API also supports OData and this post will show you how to integrate those two.."
            };

            Document _wcfCustomSecurity= new Document()
            {
                Author = "Harshal S.",
                Title = "Secure WCF Services with custom encrypted tokens",
                OnlineURL = "http://harshal.com/2014/12/13/secure-wcf-services-with-custom-encrypted-tokens/",
                Contents = @"Windows Communication Foundation framework comes with a lot of options out of the box, 
                            concerning the security logic you will apply to your services. Different bindings can be
                            used for certain kind and levels of security. Even the BasicHttpBinding binding supports
                            some types of security. There are some times though where you cannot or don’t want to use
                            WCF security available options and hence, you need to develop your own authentication logic
                            accoarding to your business needs."
            };

            _documents.Add(_oData);
            _documents.Add(_wcfCustomSecurity);

            return _documents;
        }

        public static List<Document> GetType1Documents()
        {
            List<Document> _documents = new List<Document>();

            Document _angularFeatWebAPI = new Document()
            {
                Author = "Hemal Shah",
                Title = "AngularJS feat. Web API",
                OnlineURL = "http://www.hemalgeeks.com/2015/05/angularjs-feat-web-api.html",
                Contents = @"Developing Web applications using AngularJS and Web API can be quite amuzing. You can pick 
                            this architecture in case you have in mind a web application with limitted page refreshes or
                            post backs to the server while each application’s View is based on partial data retrieved from it."
            };

            _documents.Add(_angularFeatWebAPI);

            return _documents;
        }

        public static List<Document> GetAllDocuments()
        {
            List<Document> _documents = new List<Document>();
            _documents.AddRange(GetType2Documents());
            _documents.AddRange(GetType1Documents());

            return _documents;
        }
    }
}
