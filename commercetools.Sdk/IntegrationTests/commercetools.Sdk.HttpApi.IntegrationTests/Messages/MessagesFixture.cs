﻿using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using System;
using System.Collections.Generic;
using Xunit.Abstractions;
using Type = commercetools.Sdk.Domain.Type;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Messages
{
    public class MessagesFixture : ClientFixture, IDisposable
    {

        public MessagesFixture(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
        }

        public void Dispose()
        {
        }
    }
}
