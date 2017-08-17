using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Automation.Anywhere.WebAPI.Domain;

namespace Automation.Anywhere.WebAPI.API.Core.Filters
{
    public class DocumentsReversedFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var objectContent = actionExecutedContext.Response.Content as ObjectContent;
            if (objectContent != null)
            {
                List<Document> _documents = objectContent.Value as List<Document>;
                if (_documents != null && _documents.Count > 0)
                {
                    _documents.Reverse();
                }
            }
        }
    }
}
