using System;
using System.Net.Http;

namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    /// <summary>
    /// Wrapper for predicate and Func, so if the http response meet the criteria of the predicate, then we use delegate Func to create the exception
    /// </summary>
    public class HttpResponseExceptionResponsibility
    {
        public Predicate<HttpResponseMessage> Predicate { get; set; }
        
        public Func<HttpResponseMessage, ApiException> ExceptionCreator { get; set; }

        public HttpResponseExceptionResponsibility(Predicate<HttpResponseMessage> predicate,
            Func<HttpResponseMessage, ApiException> exceptionCreator)
        {
            this.Predicate = predicate;
            this.ExceptionCreator = exceptionCreator;
        }
    }
}