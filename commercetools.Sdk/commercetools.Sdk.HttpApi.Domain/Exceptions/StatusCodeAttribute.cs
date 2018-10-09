using System;

namespace commercetools.Sdk.HttpApi.Domain
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class StatusCodeAttribute : Attribute
    {
        private string statusCode; 

        public StatusCodeAttribute(string statusCode)
        {
            this.statusCode = statusCode;
        }

        public string StatusCode
        {
            get
            {
                return this.statusCode;
            }
        }
    }
}
