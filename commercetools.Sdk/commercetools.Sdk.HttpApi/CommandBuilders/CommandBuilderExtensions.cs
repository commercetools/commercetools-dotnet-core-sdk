using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CartDiscounts;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.CustomerGroups;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.CustomObjects;
using commercetools.Sdk.Domain.DiscountCodes;
using commercetools.Sdk.Domain.InventoryEntries;
using commercetools.Sdk.Domain.Messages;
using commercetools.Sdk.Domain.OrderEdits;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.Payments;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.ProductDiscounts;
using commercetools.Sdk.Domain.ProductProjections;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.Domain.Reviews;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Domain.ShoppingLists;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.Domain.Stores;
using commercetools.Sdk.Domain.Subscriptions;
using commercetools.Sdk.Domain.TaxCategories;
using commercetools.Sdk.Domain.Zones;
using Type = commercetools.Sdk.Domain.Types.Type;

namespace commercetools.Sdk.HttpApi.CommandBuilders
{
    public static class CommandBuilderExtensions
    {
        #region IClient

        public static CommandBuilder Builder(this IClient client)
        {
            return new CommandBuilder(client);
        }

        #endregion

        #region Domains

        public static DomainCommandBuilder<Category> Categories(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<Category>(builder.Client);
        }

        public static DomainCommandBuilder<Customer> Customers(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<Customer>(builder.Client);
        }

        public static DomainCommandBuilder<Cart> Carts(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<Cart>(builder.Client);
        }

        public static DomainCommandBuilder<DiscountCode> DiscountCodes(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<DiscountCode>(builder.Client);
        }

        public static DomainCommandBuilder<CartDiscount> CartDiscounts(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<CartDiscount>(builder.Client);
        }

        public static DomainCommandBuilder<Channel> Channels(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<Channel>(builder.Client);
        }

        public static DomainCommandBuilder<CustomerGroup> CustomerGroups(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<CustomerGroup>(builder.Client);
        }

        public static DomainCommandBuilder<InventoryEntry> Inventory(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<InventoryEntry>(builder.Client);
        }

        public static DomainCommandBuilder<Message> Messages(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<Message>(builder.Client);
        }

        public static DomainCommandBuilder<Order> Orders(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<Order>(builder.Client);
        }

        public static DomainCommandBuilder<OrderEdit> OrderEdits(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<OrderEdit>(builder.Client);
        }

        public static DomainCommandBuilder<Payment> Payments(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<Payment>(builder.Client);
        }

        public static DomainCommandBuilder<ProductDiscount> ProductDiscounts(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<ProductDiscount>(builder.Client);
        }

        public static DomainCommandBuilder<ProductProjection> ProductProjections(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<ProductProjection>(builder.Client);
        }

        public static DomainCommandBuilder<Product> Products(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<Product>(builder.Client);
        }

        public static DomainCommandBuilder<ProductType> ProductTypes(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<ProductType>(builder.Client);
        }

        public static DomainCommandBuilder<Review> Reviews(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<Review>(builder.Client);
        }

        public static DomainCommandBuilder<ShippingMethod> ShippingMethods(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<ShippingMethod>(builder.Client);
        }

        public static DomainCommandBuilder<ShoppingList> ShoppingLists(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<ShoppingList>(builder.Client);
        }

        public static DomainCommandBuilder<State> States(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<State>(builder.Client);
        }

        public static DomainCommandBuilder<Store> Stores(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<Store>(builder.Client);
        }

        public static DomainCommandBuilder<Subscription> Subscriptions(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<Subscription>(builder.Client);
        }

        public static DomainCommandBuilder<TaxCategory> TaxCategories(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<TaxCategory>(builder.Client);
        }

        public static DomainCommandBuilder<Type> Types(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<Type>(builder.Client);
        }

        public static DomainCommandBuilder<Zone> Zones(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<Zone>(builder.Client);
        }

        public static DomainCommandBuilder<CustomObjectBase> CustomObjects(this CommandBuilder builder)
        {
            return new DomainCommandBuilder<CustomObjectBase>(builder.Client);
        }

        #endregion

        #region Get

        public static CommandBuilder<GetCommand<T>, T> GetById<T>(
            this DomainCommandBuilder<T> builder,
            string id,
            List<Expansion<T>> expand = null)
            where T : Resource<T>
        {
            return new CommandBuilder<GetCommand<T>, T>(builder.Client, () => new GetByIdCommand<T>(id, expand));
        }

        public static CommandBuilder<CheckCommand<T>, T> CheckById<T>(
            this DomainCommandBuilder<T> builder,
            string id)
            where T : Resource<T>, ICheckable<T>
        {
            return new CommandBuilder<CheckCommand<T>, T>(builder.Client, () => new CheckByIdCommand<T>(id));
        }

        public static CommandBuilder<InStoreCommand<T>, T> GetById<T>(
            this CommandBuilder<InStoreCommand<T>, T> builder,
            string id,
            List<Expansion<T>> expand = null)
            where T : Resource<T>
        {
            return new CommandBuilder<InStoreCommand<T>, T>(builder.Client, () =>
            {
                var inStoreCommand = builder.Build();
                inStoreCommand.InnerCommand = new GetByIdCommand<T>(id, expand);
                return inStoreCommand;
            });
        }

        public static CommandBuilder<GetCommand<T>, T> GetByKey<T>(
            this DomainCommandBuilder<T> builder,
            string key,
            List<Expansion<T>> expand = null)
            where T : Resource<T>, IKeyReferencable<T>
        {
            return new CommandBuilder<GetCommand<T>, T>(builder.Client, () => new GetByKeyCommand<T>(key, expand));
        }

        public static CommandBuilder<CheckCommand<T>, T> CheckByKey<T>(
            this DomainCommandBuilder<T> builder,
            string key)
            where T : Resource<T>, ICheckable<T>, IKeyReferencable<T>
        {
            return new CommandBuilder<CheckCommand<T>, T>(builder.Client, () => new CheckByKeyCommand<T>(key));
        }

        public static CommandBuilder<InStoreCommand<T>, T> GetByKey<T>(
            this CommandBuilder<InStoreCommand<T>, T> builder,
            string key,
            List<Expansion<T>> expand = null)
            where T : Resource<T>, IKeyReferencable<T>
        {
            return new CommandBuilder<InStoreCommand<T>, T>(builder.Client, () =>
            {
                var inStoreCommand = builder.Build();
                inStoreCommand.InnerCommand = new GetByKeyCommand<T>(key, expand);
                return inStoreCommand;
            });
        }

        #endregion

        #region Delete

        public static CommandBuilder<DeleteCommand<T>, T> DeleteById<T>(
            this DomainCommandBuilder<T> builder,
            string id,
            int version,
            List<Expansion<T>> expand = null)
            where T : Resource<T>
        {
            return new CommandBuilder<DeleteCommand<T>, T>(
                builder.Client,
                () => new DeleteByIdCommand<T>(id, version, expand));
        }

        public static CommandBuilder<DeleteCommand<T>, T> DeleteById<T>(
            this DomainCommandBuilder<T> builder,
            IVersioned<T> resource,
            List<Expansion<T>> expand = null)
            where T : Resource<T>
        {
            return new CommandBuilder<DeleteCommand<T>, T>(
                builder.Client,
                () => new DeleteByIdCommand<T>(resource.Id, resource.Version, expand));
        }

        public static CommandBuilder<InStoreCommand<T>, T> DeleteById<T>(
            this CommandBuilder<InStoreCommand<T>, T> builder,
            string id,
            int version,
            List<Expansion<T>> expand = null)
            where T : Resource<T>
        {
            return new CommandBuilder<InStoreCommand<T>, T>(builder.Client, () =>
            {
                var inStoreCommand = builder.Build();
                inStoreCommand.InnerCommand = new DeleteByIdCommand<T>(id, version, expand);
                return inStoreCommand;
            });
        }

        public static CommandBuilder<InStoreCommand<T>, T> DeleteById<T>(
            this CommandBuilder<InStoreCommand<T>, T> builder,
            IVersioned<T> resource,
            List<Expansion<T>> expand = null)
            where T : Resource<T>
        {
            return new CommandBuilder<InStoreCommand<T>, T>(builder.Client, () =>
            {
                var inStoreCommand = builder.Build();
                inStoreCommand.InnerCommand = new DeleteByIdCommand<T>(resource.Id, resource.Version, expand);
                return inStoreCommand;
            });
        }

        public static CommandBuilder<DeleteCommand<T>, T> DeleteByKey<T>(
            this DomainCommandBuilder<T> builder,
            string key,
            int version)
            where T : Resource<T>, IKeyReferencable<T>
        {
            return new CommandBuilder<DeleteCommand<T>, T>(
                builder.Client,
                () => new DeleteByKeyCommand<T>(key, version));
        }

        public static CommandBuilder<DeleteCommand<T>, T> DeleteByKey<T>(
            this DomainCommandBuilder<T> builder,
            T resource)
            where T : Resource<T>, IKeyReferencable<T>, IVersioned<T>
        {
            return new CommandBuilder<DeleteCommand<T>, T>(
                builder.Client,
                () => new DeleteByKeyCommand<T>(resource.Key, resource.Version));
        }

        public static CommandBuilder<InStoreCommand<T>, T> DeleteByKey<T>(
            this CommandBuilder<InStoreCommand<T>, T> builder,
            string key,
            int version,
            List<Expansion<T>> expand = null)
            where T : Resource<T>, IKeyReferencable<T>
        {
            return new CommandBuilder<InStoreCommand<T>, T>(builder.Client, () =>
            {
                var inStoreCommand = builder.Build();
                inStoreCommand.InnerCommand = new DeleteByKeyCommand<T>(key, version, expand);
                return inStoreCommand;
            });
        }

        public static CommandBuilder<InStoreCommand<T>, T> DeleteByKey<T>(
            this CommandBuilder<InStoreCommand<T>, T> builder,
            T resource,
            List<Expansion<T>> expand = null)
            where T : Resource<T>, IKeyReferencable<T>, IVersioned<T>
        {
            return new CommandBuilder<InStoreCommand<T>, T>(builder.Client, () =>
            {
                var inStoreCommand = builder.Build();
                inStoreCommand.InnerCommand = new DeleteByKeyCommand<T>(resource.Key, resource.Version, expand);
                return inStoreCommand;
            });
        }

        #endregion

        #region Create

        public static CommandBuilder<CreateCommand<T>, T> Create<T, TDraft>(
            this DomainCommandBuilder<T> builder,
            TDraft draft)
            where T : Resource<T>
            where TDraft : IDraft<T>
        {
            return new CommandBuilder<CreateCommand<T>, T>(builder.Client, () => new CreateCommand<T>(draft));
        }

        public static CommandBuilder<InStoreCommand<T>, T> Create<T, TDraft>(
            this CommandBuilder<InStoreCommand<T>, T> builder,
            TDraft draft)
            where T : Resource<T>
            where TDraft : IDraft<T>
        {
            return new CommandBuilder<InStoreCommand<T>, T>(builder.Client, () =>
            {
                var inStoreCommand = builder.Build();
                inStoreCommand.InnerCommand = new CreateCommand<T>(draft);
                return inStoreCommand;
            });
        }

        #endregion

        #region InStore

        public static CommandBuilder<InStoreCommand<T>, T> InStore<T>(
            this DomainCommandBuilder<T> builder,
            string storeKey)
            where T : Resource<T>, IInStoreUsable
        {
            return new CommandBuilder<InStoreCommand<T>, T>(
                builder.Client,
                () => new InStoreCommand<T>(storeKey, null));
        }

        public static CommandBuilder<InStoreCommand<T>, T> InStore<T>(
            this CommandBuilder<GetCommand<T>, T> builder,
            string storeKey)
            where T : Resource<T>, IInStoreUsable
        {
            return new CommandBuilder<InStoreCommand<T>, T>(builder.Client, () => builder.Build().InStore(storeKey));
        }

        public static CommandBuilder<InStoreCommand<T>, T> InStore<T>(
            this CommandBuilder<DeleteCommand<T>, T> builder,
            string storeKey)
            where T : Resource<T>, IInStoreUsable
        {
            return new CommandBuilder<InStoreCommand<T>, T>(builder.Client, () => builder.Build().InStore(storeKey));
        }

        public static CommandBuilder<InStoreCommand<T>, T> InStore<T>(
            this CommandBuilder<CreateCommand<T>, T> builder,
            string storeKey)
            where T : Resource<T>, IInStoreUsable
        {
            return new CommandBuilder<InStoreCommand<T>, T>(builder.Client, () => builder.Build().InStore(storeKey));
        }

        public static CommandBuilder<InStoreCommand<SignInResult<T>>, SignInResult<T>> InStore<T>(
            this CommandBuilder<SignUpCommand<T>, SignInResult<T>> builder,
            string storeKey)
            where T : Resource<T>, IInStoreUsable
        {
            return new CommandBuilder<InStoreCommand<SignInResult<T>>, SignInResult<T>>(
                builder.Client,
                () => builder.Build().InStore(storeKey));
        }

        public static CommandBuilder<InStoreCommand<SignInResult<T>>, SignInResult<T>> InStore<T>(
            this CommandBuilder<LoginCommand<T>, SignInResult<T>> builder,
            string storeKey)
            where T : Resource<T>, IInStoreUsable
        {
            return new CommandBuilder<InStoreCommand<SignInResult<T>>, SignInResult<T>>(
                builder.Client,
                () => builder.Build().InStore(storeKey));
        }

        public static CommandBuilder<InStoreCommand<T>, T> InStore<T>(
            this CommandBuilder<ResetPasswordCommand<T>, T> builder,
            string storeKey)
            where T : Resource<T>, IInStoreUsable
        {
            return new CommandBuilder<InStoreCommand<T>, T>(builder.Client, () => builder.Build().InStore(storeKey));
        }

        public static CommandBuilder<InStoreCommand<T>, T> InStore<T>(
            this CommandBuilder<VerifyEmailCommand<T>, T> builder,
            string storeKey)
            where T : Resource<T>, IInStoreUsable
        {
            return new CommandBuilder<InStoreCommand<T>, T>(builder.Client, () => builder.Build().InStore(storeKey));
        }

        public static CommandBuilder<InStoreCommand<Token<T>>, Token<T>> InStore<T>(
            this CommandBuilder<CreateTokenForPasswordResetCommand<T>, Token<T>> builder,
            string storeKey)
            where T : Resource<T>, IInStoreUsable
        {
            return new CommandBuilder<InStoreCommand<Token<T>>, Token<T>>(
                builder.Client,
                () => builder.Build().InStore(storeKey));
        }

        public static CommandBuilder<InStoreCommand<Token<T>>, Token<T>> InStore<T>(
            this CommandBuilder<CreateTokenForEmailVerificationCommand<T>, Token<T>> builder,
            string storeKey)
            where T : Resource<T>, IInStoreUsable
        {
            return new CommandBuilder<InStoreCommand<Token<T>>, Token<T>>(
                builder.Client,
                () => builder.Build().InStore(storeKey));
        }

        public static CommandBuilder<InStoreCommand<T>, T> InStore<T>(
            this CommandBuilder<UpdateCommand<T>, T> builder,
            string storeKey)
            where T : Resource<T>, IInStoreUsable
        {
            return new CommandBuilder<InStoreCommand<T>, T>(builder.Client, () => builder.Build().InStore(storeKey));
        }

        public static CommandBuilder<InStoreCommand<PagedQueryResult<T>>, PagedQueryResult<T>> InStore<T>(
            this CommandBuilder<QueryCommand<T>, PagedQueryResult<T>> builder,
            string storeKey)
            where T : Resource<T>, IInStoreUsable
        {
            return new CommandBuilder<InStoreCommand<PagedQueryResult<T>>, PagedQueryResult<T>>(
                builder.Client,
                () => builder.Build().InStore(storeKey));
        }

        #endregion

        #region Query

        public static QueryCommandBuilder<T> Query<T>(this DomainCommandBuilder<T> builder)
            where T : Resource<T>
        {
            return new QueryCommandBuilder<T>(builder.Client, (queryParams) => new QueryCommand<T>(queryParams));
        }

        public static QueryCommandBuilder<T> Where<T>(
            this QueryCommandBuilder<T> builder, Expression<Func<T, bool>> expression)
            where T : Resource<T>
        {
            if (builder.QueryParameters is IPredicateQueryable queryParameters)
            {
                queryParameters.Where.Add(new QueryPredicate<T>(expression).ToString());
            }

            return builder;
        }

        public static QueryCommandBuilder<T> Where<T>(
            this QueryCommandBuilder<T> builder, string expression)
            where T : Resource<T>
        {
            if (builder.QueryParameters is IPredicateQueryable queryParameters)
            {
                queryParameters.Where.Add(expression);
            }

            return builder;
        }

        public static QueryCommandBuilder<T> Expand<T>(
            this QueryCommandBuilder<T> builder, Expression<Func<T, Reference>> expression)
            where T : Resource<T>
        {
            if (builder.QueryParameters is IExpandable queryParameters)
            {
                queryParameters.Expand.Add(new Expansion<T>(expression).ToString());
            }

            return builder;
        }

        public static QueryCommandBuilder<T> Expand<T>(
            this QueryCommandBuilder<T> builder, string expression)
            where T : Resource<T>
        {
            if (builder.QueryParameters is IExpandable queryParameters)
            {
                queryParameters.Expand.Add(expression);
            }

            return builder;
        }

        public static QueryCommandBuilder<T> Sort<T>(
            this QueryCommandBuilder<T> builder,
            Expression<Func<T, IComparable>> expression,
            SortDirection sortDirection = SortDirection.Ascending)
            where T : Resource<T>
        {
            if (builder.QueryParameters is ISortable queryParameters)
            {
                queryParameters.Sort.Add(new Sort<T>(expression, sortDirection).ToString());
            }

            return builder;
        }

        public static QueryCommandBuilder<T> Sort<T>(
            this QueryCommandBuilder<T> builder, string expression)
            where T : Resource<T>
        {
            if (builder.QueryParameters is ISortable queryParameters)
            {
                queryParameters.Sort.Add(expression);
            }

            return builder;
        }

        public static QueryCommandBuilder<T> SetLimit<T>(
            this QueryCommandBuilder<T> builder, int limit)
            where T : Resource<T>
        {
            if (builder.QueryParameters is IPageable queryParameters)
            {
                queryParameters.Limit = limit;
            }

            return builder;
        }

        public static QueryCommandBuilder<T> SetOffset<T>(
            this QueryCommandBuilder<T> builder, int offset)
            where T : Resource<T>
        {
            if (builder.QueryParameters is IPageable queryParameters)
            {
                queryParameters.Offset = offset;
            }

            return builder;
        }

        #endregion

        #region Update

        public static UpdateCommandBuilder<T> UpdateById<T>(
            this DomainCommandBuilder<T> builder,
            string id,
            int version)
            where T : Resource<T>
        {
            return new UpdateCommandBuilder<T>(
                builder.Client,
                (actions) => new UpdateByIdCommand<T>(id, version, actions));
        }

        public static UpdateCommandBuilder<T> UpdateById<T>(
            this DomainCommandBuilder<T> builder,
            IVersioned<T> resource)
            where T : Resource<T>
        {
            return new UpdateCommandBuilder<T>(
                builder.Client,
                (actions) => new UpdateByIdCommand<T>(resource.Id, resource.Version, actions));
        }

        public static UpdateCommandBuilder<T> UpdateByKey<T>(
            this DomainCommandBuilder<T> builder,
            string key,
            int version)
            where T : Resource<T>, IKeyReferencable<T>, IVersioned<T>
        {
            return new UpdateCommandBuilder<T>(
                builder.Client,
                (actions) => new UpdateByKeyCommand<T>(key, version, actions));
        }

        public static UpdateCommandBuilder<T> UpdateByKey<T>(
            this DomainCommandBuilder<T> builder,
            T resource)
            where T : Resource<T>, IKeyReferencable<T>, IVersioned<T>
        {
            return new UpdateCommandBuilder<T>(
                builder.Client,
                (actions) => new UpdateByKeyCommand<T>(resource.Key, resource.Version, actions));
        }

        public static UpdateCommandBuilder<T> AddAction<T>(
            this UpdateCommandBuilder<T> builder,
            UpdateAction<T> action)
            where T : Resource<T>
        {
            builder.Actions.Add(action);
            return builder;
        }

        public static UpdateCommandBuilder<T> AddAction<T>(
            this UpdateCommandBuilder<T> builder,
            Func<UpdateAction<T>> action)
            where T : Resource<T>
        {
            var addedAction = action.Invoke();
            builder.Actions.Add(addedAction);
            return builder;
        }

        #endregion

        #region Customers

        public static CommandBuilder<SignUpCommand<T>, SignInResult<T>> SignUp<T>(
            this DomainCommandBuilder<T> builder,
            CustomerDraft draft)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<SignUpCommand<T>, SignInResult<T>>(
                builder.Client,
                () => new SignUpCustomerCommand(draft) as SignUpCommand<T>);
        }

        public static CommandBuilder<InStoreCommand<SignInResult<T>>, SignInResult<T>> SignUp<T>(
            this CommandBuilder<InStoreCommand<T>, T> builder,
            CustomerDraft draft)
            where T : Resource<T>
        {
            return new CommandBuilder<InStoreCommand<SignInResult<T>>, SignInResult<T>>(
                builder.Client, () =>
                {
                    var inStoreCommand = builder.Build();
                    var innerCommand = new SignUpCustomerCommand(draft) as SignUpCommand<T>;
                    var newInStoreCommand = new InStoreCommand<SignInResult<T>>(inStoreCommand.StoreKey, innerCommand);
                    return newInStoreCommand;
                });
        }

        public static CommandBuilder<LoginCommand<T>, SignInResult<T>> Login<T>(
            this DomainCommandBuilder<T> builder,
            string email,
            string password)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<LoginCommand<T>, SignInResult<T>>(
                builder.Client,
                () => new LoginCustomerCommand(email, password) as LoginCommand<T>);
        }

        public static CommandBuilder<InStoreCommand<SignInResult<T>>, SignInResult<T>> Login<T>(
            this CommandBuilder<InStoreCommand<T>, T> builder,
            string email,
            string password)
            where T : Resource<T>
        {
            return new CommandBuilder<InStoreCommand<SignInResult<T>>, SignInResult<T>>(
                builder.Client, () =>
                {
                    var inStoreCommand = builder.Build();
                    var innerCommand = new LoginCustomerCommand(email, password) as LoginCommand<T>;
                    var newInStoreCommand = new InStoreCommand<SignInResult<T>>(inStoreCommand.StoreKey, innerCommand);
                    return newInStoreCommand;
                });
        }

        public static CommandBuilder<ChangePasswordCommand<T>, T> ChangePassword<T>(
            this DomainCommandBuilder<T> builder,
            string id,
            int version,
            string currentPassword,
            string newPassword)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<ChangePasswordCommand<T>, T>(
                builder.Client,
                () => new ChangeCustomerPasswordCommand(id, version, currentPassword, newPassword) as
                    ChangePasswordCommand<T>);
        }


        public static CommandBuilder<InStoreCommand<T>, T> InStore<T>(
            this CommandBuilder<ChangePasswordCommand<T>, T> builder,
            string storeKey)
            where T : Resource<T>, IInStoreUsable
        {
            return new CommandBuilder<InStoreCommand<T>, T>(builder.Client, () => builder.Build().InStore(storeKey));
        }

        public static CommandBuilder<InStoreCommand<T>, T> ChangePassword<T>(
            this CommandBuilder<InStoreCommand<T>, T> builder,
            string id,
            int version,
            string currentPassword,
            string newPassword)
            where T : Resource<T>
        {
            return new CommandBuilder<InStoreCommand<T>, T>(
                builder.Client, () =>
                {
                    var inStoreCommand = builder.Build();
                    var innerCommand = new ChangeCustomerPasswordCommand(id, version, currentPassword, newPassword) as ChangePasswordCommand<T>;
                    var newInStoreCommand = new InStoreCommand<T>(inStoreCommand.StoreKey, innerCommand);
                    return newInStoreCommand;
                });
        }

        public static CommandBuilder<ChangePasswordCommand<T>, T> ChangePassword<T>(
            this DomainCommandBuilder<T> builder,
            IVersioned<T> resource,
            string currentPassword,
            string newPassword)
            where T : Resource<T>, ISignupable, IVersioned<T>
        {
            return new CommandBuilder<ChangePasswordCommand<T>, T>(
                builder.Client,
                () => new ChangeCustomerPasswordCommand(resource.Id, resource.Version, currentPassword, newPassword) as
                    ChangePasswordCommand<T>);
        }

        public static CommandBuilder<InStoreCommand<T>, T> ChangePassword<T>(
            this CommandBuilder<InStoreCommand<T>, T> builder,
            IVersioned<T> resource,
            string currentPassword,
            string newPassword)
            where T : Resource<T>
        {
            return new CommandBuilder<InStoreCommand<T>, T>(
                builder.Client, () =>
                {
                    var inStoreCommand = builder.Build();
                    var innerCommand = new ChangeCustomerPasswordCommand(resource.Id, resource.Version, currentPassword, newPassword) as ChangePasswordCommand<T>;
                    var newInStoreCommand = new InStoreCommand<T>(inStoreCommand.StoreKey, innerCommand);
                    return newInStoreCommand;
                });
        }

        public static CommandBuilder<CreateTokenForPasswordResetCommand<T>, Token<T>>
            CreateTokenForPasswordResetting<T>(
                this DomainCommandBuilder<T> builder,
                string email,
                int? timeToLiveMinutes = null)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<CreateTokenForPasswordResetCommand<T>, Token<T>>(
                builder.Client,
                () => new CreateTokenForCustomerPasswordResetCommand(email, timeToLiveMinutes) as
                    CreateTokenForPasswordResetCommand<T>);
        }

        public static CommandBuilder<InStoreCommand<Token<T>>, Token<T>>
            CreateTokenForPasswordResetting<T>(
                this CommandBuilder<InStoreCommand<T>, T> builder,
                string email,
                int? timeToLiveMinutes = null)
            where T : Resource<T>
        {
            return new CommandBuilder<InStoreCommand<Token<T>>, Token<T>>(
                builder.Client, () =>
                {
                    var inStoreCommand = builder.Build();
                    var innerCommand =
                        new CreateTokenForCustomerPasswordResetCommand(email, timeToLiveMinutes) as
                            CreateTokenForPasswordResetCommand<T>;
                    var newInStoreCommand = new InStoreCommand<Token<T>>(inStoreCommand.StoreKey, innerCommand);
                    return newInStoreCommand;
                });
        }

        public static CommandBuilder<CreateTokenForEmailVerificationCommand<T>, Token<T>>
            CreateTokenForEmailVerification<T>(
                this DomainCommandBuilder<T> builder,
                string id,
                int timeToLiveMinutes,
                int? version = null)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<CreateTokenForEmailVerificationCommand<T>, Token<T>>(
                builder.Client,
                () => new CreateTokenForCustomerEmailVerificationCommand(id, timeToLiveMinutes, version) as
                    CreateTokenForEmailVerificationCommand<T>);
        }

        public static CommandBuilder<InStoreCommand<Token<T>>, Token<T>>
            CreateTokenForEmailVerification<T>(
                this CommandBuilder<InStoreCommand<T>, T> builder,
                string id,
                int timeToLiveMinutes,
                int? version = null)
            where T : Resource<T>
        {
            return new CommandBuilder<InStoreCommand<Token<T>>, Token<T>>(
                builder.Client, () =>
                {
                    var inStoreCommand = builder.Build();
                    var innerCommand =
                        new CreateTokenForCustomerEmailVerificationCommand(id, timeToLiveMinutes, version) as
                            CreateTokenForEmailVerificationCommand<T>;
                    var newInStoreCommand = new InStoreCommand<Token<T>>(inStoreCommand.StoreKey, innerCommand);
                    return newInStoreCommand;
                });
        }

        public static CommandBuilder<GetCommand<T>, T> GetByEmailToken<T>(
            this DomainCommandBuilder<T> builder,
            string emailToken)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<GetCommand<T>, T>(
                builder.Client,
                () => new GetCustomerByEmailTokenCommand(emailToken) as GetCommand<T>);
        }

        public static CommandBuilder<GetCommand<T>, T> GetByEmailToken<T>(
            this CommandBuilder<CreateTokenForEmailVerificationCommand<T>, Token<T>> builder)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<GetCommand<T>, T>(builder.Client, () =>
            {
                var result = builder.ExecuteAsync().Result as CustomerToken;
                var emailToken = result?.Value;
                return new GetCustomerByEmailTokenCommand(emailToken) as GetCommand<T>;
            });
        }

        public static CommandBuilder<GetCommand<T>, T> GetByPasswordToken<T>(
            this DomainCommandBuilder<T> builder,
            string passwordToken)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<GetCommand<T>, T>(
                builder.Client,
                () => new GetCustomerByPasswordTokenCommand(passwordToken) as GetCommand<T>);
        }

        public static CommandBuilder<GetCommand<T>, T> GetByPasswordToken<T>(
            this CommandBuilder<CreateTokenForPasswordResetCommand<T>, Token<T>> builder)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<GetCommand<T>, T>(builder.Client, () =>
            {
                var result = builder.ExecuteAsync().Result as CustomerToken;
                var passwordToken = result?.Value;
                return new GetCustomerByPasswordTokenCommand(passwordToken) as GetCommand<T>;
            });
        }

        public static CommandBuilder<ResetPasswordCommand<T>, T> ResetPassword<T>(
            this DomainCommandBuilder<T> builder,
            string tokenValue,
            string newPassword,
            int? version = null)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<ResetPasswordCommand<T>, T>(
                builder.Client,
                () => new ResetCustomerPasswordCommand(tokenValue, newPassword, version) as ResetPasswordCommand<T>);
        }

        public static CommandBuilder<ResetPasswordCommand<T>, T> ResetPassword<T>(
            this CommandBuilder<CreateTokenForPasswordResetCommand<T>, Token<T>> builder,
            string newPassword,
            int? version = null)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<ResetPasswordCommand<T>, T>(builder.Client, () =>
            {
                var result = builder.ExecuteAsync().Result as CustomerToken;
                var passwordToken = result?.Value;
                return new ResetCustomerPasswordCommand(passwordToken, newPassword, version) as ResetPasswordCommand<T>;
            });
        }

        public static CommandBuilder<VerifyEmailCommand<T>, T> VerifyEmail<T>(
            this DomainCommandBuilder<T> builder,
            string tokenValue)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<VerifyEmailCommand<T>, T>(
                builder.Client,
                () => new VerifyCustomerEmailCommand(tokenValue) as VerifyEmailCommand<T>);
        }

        public static CommandBuilder<VerifyEmailCommand<T>, T> VerifyEmail<T>(
            this CommandBuilder<CreateTokenForEmailVerificationCommand<T>, Token<T>> builder)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<VerifyEmailCommand<T>, T>(builder.Client, () =>
            {
                var result = builder.ExecuteAsync().Result as CustomerToken;
                var emailToken = result?.Value;
                return new VerifyCustomerEmailCommand(emailToken) as VerifyEmailCommand<T>;
            });
        }

        #endregion

        #region Carts

        public static CommandBuilder<GetCommand<Cart>, Cart> GetByCustomerId(
            this DomainCommandBuilder<Cart> builder,
            string customerId,
            List<Expansion<Cart>> expand = null)
        {
            return new CommandBuilder<GetCommand<Cart>, Cart>(
                builder.Client,
                () => new GetCartByCustomerIdCommand(customerId, expand) as GetCommand<Cart>);
        }

        public static CommandBuilder<InStoreCommand<Cart>, Cart> GetByCustomerId(
            this CommandBuilder<InStoreCommand<Cart>, Cart> builder,
            string customerId,
            List<Expansion<Cart>> expand = null)
        {
            return new CommandBuilder<InStoreCommand<Cart>, Cart>(builder.Client, () =>
            {
                var inStoreCommand = builder.Build();
                inStoreCommand.InnerCommand = new GetCartByCustomerIdCommand(customerId, expand);
                return inStoreCommand;
            });
        }

        public static CommandBuilder<ReplicateCommand<Cart>, Cart> Replicate(
            this DomainCommandBuilder<Cart> builder,
            IReplicaDraft<Cart> replica)
        {
            return new CommandBuilder<ReplicateCommand<Cart>, Cart>(
                builder.Client,
                () => new ReplicateCartCommand(replica) as ReplicateCommand<Cart>);
        }

        public static CommandBuilder<InStoreCommand<Cart>, Cart> InStore(
            this CommandBuilder<ReplicateCommand<Cart>, Cart> builder,
            string storeKey)
        {
            return new CommandBuilder<InStoreCommand<Cart>, Cart>(builder.Client, () => builder.Build().InStore(storeKey));
        }

        #endregion

        #region Orders

        public static CommandBuilder<GetCommand<Order>, Order> GetByOrderNumber(
            this DomainCommandBuilder<Order> builder,
            string orderNumber,
            List<Expansion<Order>> expand = null)
        {
            return new CommandBuilder<GetCommand<Order>, Order>(
                builder.Client,
                () => new GetOrderByOrderNumberCommand(orderNumber, expand) as GetCommand<Order>);
        }

        public static CommandBuilder<InStoreCommand<Order>, Order> GetByOrderNumber(
            this CommandBuilder<InStoreCommand<Order>, Order> builder,
            string orderNumber,
            List<Expansion<Order>> expand = null)
        {
            return new CommandBuilder<InStoreCommand<Order>, Order>(builder.Client, () =>
            {
                var inStoreCommand = builder.Build();
                inStoreCommand.InnerCommand = new GetOrderByOrderNumberCommand(orderNumber, expand);
                return inStoreCommand;
            });
        }

        public static CommandBuilder<DeleteCommand<Order>, Order> DeleteByOrderNumber(
            this DomainCommandBuilder<Order> builder,
            string orderNumber,
            int version,
            List<Expansion<Order>> expand = null)
        {
            return new CommandBuilder<DeleteCommand<Order>, Order>(
                builder.Client,
                () => new DeleteByOrderNumberCommand(orderNumber, version, expand) as DeleteCommand<Order>);
        }

        public static CommandBuilder<InStoreCommand<Order>, Order> DeleteByOrderNumber(
            this CommandBuilder<InStoreCommand<Order>, Order> builder,
            string orderNumber,
            int version,
            List<Expansion<Order>> expand = null)
        {
            return new CommandBuilder<InStoreCommand<Order>, Order>(builder.Client, () =>
            {
                var inStoreCommand = builder.Build();
                inStoreCommand.InnerCommand = new DeleteByOrderNumberCommand(orderNumber, version, expand);
                return inStoreCommand;
            });
        }

        #endregion

        #region OrderEdits

        public static CommandBuilder<ApplyResourceEditCommand<OrderEdit>, OrderEdit> ApplyOrderEdit(
            this DomainCommandBuilder<OrderEdit> builder,
            string id,
            int editVersion,
            int resourceVersion)
        {
            return new CommandBuilder<ApplyResourceEditCommand<OrderEdit>, OrderEdit>(
                builder.Client,
                () => new ApplyOrderEditCommand(
                    id,
                    editVersion,
                    resourceVersion) as ApplyResourceEditCommand<OrderEdit>);
        }

        #endregion

        #region Products

        public static CommandBuilder<UploadImageCommand<Product>, Product> UploadProductImage(
            this DomainCommandBuilder<Product> builder,
            Guid id,
            Stream image,
            string contentType)
        {
            return new CommandBuilder<UploadImageCommand<Product>, Product>(
                builder.Client,
                () => new UploadProductImageCommand(id, image, contentType) as UploadImageCommand<Product>);
        }

        #endregion

        #region CustomObjects

        public static QueryCommandBuilder<CustomObject> QueryByContainer(this DomainCommandBuilder<CustomObjectBase> builder, string container)
        {
            return new QueryCommandBuilder<CustomObject>(builder.Client, (queryParams) => new QueryByContainerCommand<CustomObject>(container, queryParams));
        }

        #endregion
    }
}