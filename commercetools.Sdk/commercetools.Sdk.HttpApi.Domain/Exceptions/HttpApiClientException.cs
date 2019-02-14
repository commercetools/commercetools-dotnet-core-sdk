using commercetools.Sdk.HttpApi.Domain.Exceptions;

namespace commercetools.Sdk.HttpApi.Domain
{
    using System;
    using System.Collections.Generic;

    public class HttpApiClientException : Exception
    {
        public int StatusCode { get; set; }
        public List<Error> Errors { get; set; }

        public HttpApiClientException(string message) : base(message)
        {
            
        }

        public HttpApiClientException(string message, int statusCode, List<Error> errors = null, string body = null) : this(message)
        {
            StatusCode = statusCode;
            Errors = errors;
        }
        /// <summary>
        /// Create Exception from the json response
        /// </summary>
        /// <param name="response"></param>
        public HttpApiClientException(HttpApiErrorResponse response) :this(response.Message)
        {
            this.StatusCode = response.StatusCode;
            this.Errors = response.Errors;
        }
        
    }
}