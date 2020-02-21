using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Errors
{
    /// <summary>
    /// Error Response
    /// </summary>
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<Error> Errors { get; set; }
    }
}