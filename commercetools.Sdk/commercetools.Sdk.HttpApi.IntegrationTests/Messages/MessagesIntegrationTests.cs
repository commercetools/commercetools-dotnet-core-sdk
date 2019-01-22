using System;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Categories;
using Xunit;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Messages
{
    [Collection("Integration Tests")]
    public class MessagesIntegrationTests
    {
        private readonly MessagesFixture messagesFixture;

        public MessagesIntegrationTests(MessagesFixture messagesFixture)
        {
            this.messagesFixture = messagesFixture;
        }
        
        /// <summary>
        /// Get Message By Id
        /// </summary>
        [Fact]
        public void GetMessageById()
        {
           
        }
    }
}