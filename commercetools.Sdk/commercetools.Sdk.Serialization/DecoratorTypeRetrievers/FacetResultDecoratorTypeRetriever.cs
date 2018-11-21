using commercetools.Sdk.Domain;
using commercetools.Sdk.HttpApi.Domain;
using commercetools.Sdk.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Serialization
{
    public class FacetResultDecoratorTypeRetriever : DecoratorTypeRetriever<FacetResult, FacetResultTypeAttribute>
    {
        public FacetResultDecoratorTypeRetriever(IRegisteredTypeRetriever registeredTypeRetriever) : base(registeredTypeRetriever) { }
    }
}
