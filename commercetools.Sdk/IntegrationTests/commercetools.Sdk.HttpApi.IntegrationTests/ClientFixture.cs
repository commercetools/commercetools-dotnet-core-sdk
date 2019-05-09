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
        private readonly IServiceProvider serviceProvider;
        private readonly IConfiguration configuration;

        public ClientFixture(IMessageSink diagnosticMessageSink)
        {
            //services.AddLogging(configure => configure.AddConsole());
            this.configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.test.json").
                AddJsonFile("appsettings.test.Development.json", true).
                // https://www.jerriepelser.com/blog/aspnet-core-no-more-worries-about-checking-in-secrets/
                AddEnvironmentVariables().
                AddUserSecrets<ClientFixture>().
                Build();

            var containerType = Enum.Parse<ContainerType>(configuration.GetValue("Container", "BuiltIn"));
            diagnosticMessageSink.OnMessage(new DiagnosticMessage("Use container {0}", containerType.ToString()));
            switch (containerType)
            {
                case ContainerType.BuiltIn:
                    var services = new ServiceCollection();
                    services.UseCommercetools(configuration, "Client", TokenFlow.ClientCredentials);
                    this.serviceProvider = services.BuildServiceProvider();
                    break;
                case ContainerType.SimpleInjector:
                    var container = new Container();
                    container.UseCommercetools(configuration, "Client", TokenFlow.ClientCredentials);
                    this.serviceProvider = container;
                    break;
            }
        }

        public T GetService<T>()
        {
            return this.serviceProvider.GetService<T>();
        }

        public IClientConfiguration GetClientConfiguration(string name)
        {
            return this.configuration.GetSection(name).Get<ClientConfiguration>();
        }
    }
}
