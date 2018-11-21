using commercetools.Sdk.HttpApi.Domain;
using commercetools.Sdk.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Serialization.DecoratorTypeRetrievers
{
    public class ErrorDecoratorTypeRetriever : DecoratorTypeRetriever<Error, ErrorTypeAttribute>
    {
        public ErrorDecoratorTypeRetriever(IRegisteredTypeRetriever registeredTypeRetriever) : base(registeredTypeRetriever) { }
    }
}
