using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    /// <summary>
    /// Write Exceptions to the output in tests
    /// </summary>
    public class CustomDelegatingHandler  : DelegatingHandler, ICustomDelegatingHandler
    {
        //private readonly ITestOutputHelper outputHelper;
        
        public CustomDelegatingHandler()
        {
            //ITestOutputHelper output
            //this.outputHelper = output;
        }
        
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            if (response != null)
            {
              
                // Show Errors
                if (!response.IsSuccessStatusCode)
                {
                    String body = response.Content.ReadAsStringAsync().Result;
                    throw new Exception($"Request Url: {request.RequestUri} \r\n Response Status Code: {response.StatusCode} \r\n Response Reason: {response.ReasonPhrase} \r\n Response {body}");
                    //outputHelper.WriteLine($"Request Url: {request.RequestUri} \r\n Response Status Code: {response.StatusCode} \r\n Response Reason: {response.ReasonPhrase}");
                }
            }
            return response;
        }
    }
}