using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using commercetools.Sdk.HttpApi.Domain;
using commercetools.Sdk.Serialization;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class Class1
    {
        [Fact]
        public void SmallTest()
        {
            Type type = typeof(DecoratorTypeRetriever<>);
            Type genericType = typeof(DecoratorTypeRetriever<Error>);
            Type concreteType = typeof(ErrorDecoratorTypeRetriever);
            Assembly assembly = Assembly.GetAssembly(type);
            var typesToRegister = type.GetAllRegisteredTypes(assembly);
            bool result = type.IsAssignableFrom(concreteType);
            bool result2 = genericType.IsAssignableFrom(concreteType);
        }
    }
}
