using System;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Stores;

namespace commercetools.Sdk.Client
{
    /// <summary>
    /// It's a Wrapper for other command, to support in-Store
    /// </summary>
    /// <typeparam name="T">type of the result of inner command</typeparam>
    public class InStoreCommand<T> : Command<T>, IInStoreCommand, IDecoratorCommand
    {
        public InStoreCommand(string storeKey, ICommand<T> innerCommand)
        {
            this.StoreKey = storeKey;
            this.InnerCommand = innerCommand;
        }

        public InStoreCommand(IKeyReferencable<Store> storeRef, ICommand<T> innerCommand)
        {
            this.StoreKey = storeRef.Key;
            this.InnerCommand = innerCommand;
        }

        public string StoreKey { get; }

        public ICommand<T> InnerCommand { get; set; }

        public override Type ResourceType => InnerCommand.ResourceType;
    }
}