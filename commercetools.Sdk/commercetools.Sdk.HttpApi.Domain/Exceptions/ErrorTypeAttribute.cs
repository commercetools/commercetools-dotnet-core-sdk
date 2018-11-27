using commercetools.Sdk.Domain;
using System;

namespace commercetools.Sdk.HttpApi.Domain
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ErrorTypeAttribute : TypeMarkerAttribute
    {
        public ErrorTypeAttribute(string statusCode)
        {
            this.Value = statusCode;
        }
    }
}