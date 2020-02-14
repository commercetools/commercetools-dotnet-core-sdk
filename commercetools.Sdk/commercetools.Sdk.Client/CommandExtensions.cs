using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Domain.Stores;

namespace commercetools.Sdk.Client
{
    public static class CommandExtensions
    {
        public static InStoreCommand<T> InStore<T>(this ICommand<T> command, string key)
            where T : IInStoreUsable
        {
            return new InStoreCommand<T>(key, command);
        }

        public static InStoreCommand<T> InStore<T>(this ICommand<T> command, IKeyReferencable<Store> storeRef)
            where T : IInStoreUsable
        {
            return new InStoreCommand<T>(storeRef, command);
        }

        public static InStoreCommand<PagedQueryResult<T>> InStore<T>(this ICommand<PagedQueryResult<T>> command, string key)
            where T : IInStoreUsable
        {
            return new InStoreCommand<PagedQueryResult<T>>(key, command);
        }

        public static InStoreCommand<PagedQueryResult<T>> InStore<T>(this ICommand<PagedQueryResult<T>> command, IKeyReferencable<Store> storeRef)
            where T : IInStoreUsable
        {
            return new InStoreCommand<PagedQueryResult<T>>(storeRef.Key, command);
        }

        public static InStoreCommand<SignInResult<T>> InStore<T>(this ICommand<SignInResult<T>> command, string key)
            where T : IInStoreUsable
        {
            return new InStoreCommand<SignInResult<T>>(key, command);
        }

        public static InStoreCommand<SignInResult<T>> InStore<T>(this ICommand<SignInResult<T>> command, IKeyReferencable<Store> storeRef)
            where T : IInStoreUsable
        {
            return new InStoreCommand<SignInResult<T>>(storeRef.Key, command);
        }

        public static InStoreCommand<Token<T>> InStore<T>(this ICommand<Token<T>> command, string key)
            where T : IInStoreUsable
        {
            return new InStoreCommand<Token<T>>(key, command);
        }

        public static InStoreCommand<Token<T>> InStore<T>(this ICommand<Token<T>> command, IKeyReferencable<Store> storeRef)
            where T : IInStoreUsable
        {
            return new InStoreCommand<Token<T>>(storeRef.Key, command);
        }

        public static InStoreCommand<PagedQueryResult<ShippingMethod>> InStore(this GetShippingMethodsForCartCommand command, string key)
        {
            return new InStoreCommand<PagedQueryResult<ShippingMethod>>(key, command);
        }

        public static InStoreCommand<PagedQueryResult<ShippingMethod>> InStore(this GetShippingMethodsForCartCommand command, IKeyReferencable<Store> storeRef)
        {
            return new InStoreCommand<PagedQueryResult<ShippingMethod>>(storeRef.Key, command);
        }
    }
}