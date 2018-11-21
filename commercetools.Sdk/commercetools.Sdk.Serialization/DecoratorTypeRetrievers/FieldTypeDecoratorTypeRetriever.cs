using commercetools.Sdk.Domain;
using commercetools.Sdk.HttpApi.Domain;
using commercetools.Sdk.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Serialization.DecoratorTypeRetrievers
{
    public class FieldTypeDecoratorTypeRetriever : DecoratorTypeRetriever<FieldType, FieldTypeAttribute>
    {
        public FieldTypeDecoratorTypeRetriever(IRegisteredTypeRetriever registeredTypeRetriever) : base(registeredTypeRetriever) { }
    }
}
