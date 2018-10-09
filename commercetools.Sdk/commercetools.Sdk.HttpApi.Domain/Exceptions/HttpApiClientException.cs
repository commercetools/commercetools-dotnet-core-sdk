namespace commercetools.Sdk.HttpApi.Domain
{
    using System;
    using System.Collections.Generic;

    public class HttpApiClientException : Exception
    {
        public int StatusCode { get; set; }
        public new string Message { get; set; }
        public List<Error> Errors { get; set; }
    }
}