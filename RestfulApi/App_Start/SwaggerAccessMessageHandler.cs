using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RestfulApi
{
    /// <summary>
    /// 
    /// </summary>
    public class SwaggerAccessMessageHandler : DelegatingHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (IsSwagger(request) && !Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Redirect);

                // Redirect to login URL
                string uri = string.Format("{0}://{1}", request.RequestUri.Scheme, request.RequestUri.Authority);
                response.Headers.Location = new Uri(uri);
                return Task.FromResult(response);
            }
            else
            {
                return base.SendAsync(request, cancellationToken);
            }
        }

        bool IsSwagger(HttpRequestMessage request)
        {
            return request.RequestUri.PathAndQuery.Contains("/swagger");
        }
    }
}