using System.Collections.Generic;

namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    /// <summary>
    /// Http API Error Response
    /// </summary>
    public class HttpApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<Error> Errors { get; set; }
    }
}