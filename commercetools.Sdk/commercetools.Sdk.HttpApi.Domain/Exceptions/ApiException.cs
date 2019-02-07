using System;
using System.Net.Http;
using System.Text;

namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    /// <summary>
    /// Wrapper type of any api exception could happen while calling api service like (client exception, server exception, internet exception, ..etc)
    /// </summary>
    public class ApiException : Exception
    {
        #region Properites

        public HttpRequestMessage Request { get; set; }
        
        public HttpResponseMessage Response { get; set; }
        
        public string ProjectKey { get; set; }
        
        #endregion

        #region Constructors

        public ApiException()
        {
            
        }

        public ApiException(HttpRequestMessage request, HttpResponseMessage response)
        {
            Request = request;
            Response = response;
        }

        #endregion

        public override string Message => this.GetExceptionMessage();

        private string GetExceptionMessage()
        {
            if (Request == null || Response == null)
                return "";
            return $"API Exception Message at {Request.RequestUri}";
        }
    }
}