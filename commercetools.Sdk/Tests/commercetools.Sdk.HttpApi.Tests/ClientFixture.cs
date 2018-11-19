using commercetools.Sdk.Client;
using commercetools.Sdk.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class ClientFixture
    {
        private ServiceProvider serviceProvider;

        public ClientFixture()
        {
            var services = new ServiceCollection();
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            services.UseSerialization();
            services.UseHttpApiWithClientCredentials(configuration);
            serviceProvider = services.BuildServiceProvider();
        }

        public T GetService<T>()
        {
            return this.serviceProvider.GetService<T>();
        }
    }
}
