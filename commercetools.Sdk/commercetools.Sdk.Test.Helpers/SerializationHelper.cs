using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Test.Helpers
{
    public static class SerializationHelper
    {
        public static ISerializerService SerializerService
        {
            get {
                var services = new ServiceCollection();
                services.UseSerialization();
                var serviceProvider = services.BuildServiceProvider();
                return serviceProvider.GetService<ISerializerService>();
            }
        }
    }
}
