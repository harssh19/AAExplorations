using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automation.Anywhere.WebAPI.API.Core.MessageHandlers
{
    public class HeaderAppenderHandler : DelegatingHandler
    {
        async protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            response.Headers.Add("X-WebAPI-Header", "Web API Unit testing in Harshal's documentvault.");
            return response;
        }
    }
}
