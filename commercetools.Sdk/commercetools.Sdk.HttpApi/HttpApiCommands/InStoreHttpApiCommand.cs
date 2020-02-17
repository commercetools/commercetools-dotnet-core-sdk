using System;
using System.Net.Http;
using commercetools.Sdk.Client;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class InStoreHttpApiCommand<T> : IHttpApiCommandGeneric<InStoreCommand<T>, T>
    {
        private readonly string storeKey;
        private readonly IHttpApiCommand command;

        public InStoreHttpApiCommand(InStoreCommand<T> inStoreDecoratorCommand, IHttpApiCommandFactory factory)
        {
            this.storeKey = inStoreDecoratorCommand.StoreKey;
            this.command = factory.Create(inStoreDecoratorCommand.InnerCommand);
        }

        public HttpRequestMessage HttpRequestMessage
        {
            get
            {
                var requestMessage = command.HttpRequestMessage;
                var uri = new Uri($"in-store/key={storeKey}/{requestMessage.RequestUri}", UriKind.Relative);
                requestMessage.RequestUri = uri;

                return requestMessage;
            }
        }
    }
}