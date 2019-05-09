﻿using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using commercetools.Sdk.DependencyInjection;
using commercetools.Sdk.HttpApi.Tokens;
using commercetools.Sdk.SimpleInjector;
using SimpleInjector;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    public class ClientFixture
    {
        public ServiceProviderFixture ServiceProviderFixture { get; }

        public ClientFixture(ServiceProviderFixture serviceProviderFixture)
        {
            this.ServiceProviderFixture = serviceProviderFixture;
        }

        public T GetService<T>()
        {
            return this.ServiceProviderFixture.GetService<T>();
        }

        public IClientConfiguration GetClientConfiguration(string name)
        {
            return this.ServiceProviderFixture.GetClientConfiguration(name);
        }
    }
}
