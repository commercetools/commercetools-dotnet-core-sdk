using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.DiscountCodes;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.HttpApi.CommandBuilders
{
    public static class CommandBuilderExtensions
    {
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

        #endregion

        #region Get

        public static CommandBuilder<GetCommand<T>, T> GetById<T>(this DomainCommandBuilder<T> builder, string id, List<Expansion<T>> expand = null)
            where T : Resource<T>
        {
            return new CommandBuilder<GetCommand<T>, T>(builder.Client, () => new GetByIdCommand<T>(id, expand));
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

        public static CommandBuilder<GetCommand<T>, T> GetByKey<T>(this DomainCommandBuilder<T> builder, string key, List<Expansion<T>> expand = null)
            where T : Resource<T>, IKeyReferencable<T>
        {
            return new CommandBuilder<GetCommand<T>, T>(builder.Client, () => new GetByKeyCommand<T>(key, expand));
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

        public static CommandBuilder<DeleteCommand<T>, T> DeleteById<T>(this DomainCommandBuilder<T> builder, string id, int version)
            where T : Resource<T>
        {
            return new CommandBuilder<DeleteCommand<T>, T>(builder.Client, () => new DeleteByIdCommand<T>(id, version));
        }

        public static CommandBuilder<DeleteCommand<T>, T> DeleteByKey<T>(this DomainCommandBuilder<T> builder, string key, int version)
            where T : Resource<T>, IKeyReferencable<T>
        {
            return new CommandBuilder<DeleteCommand<T>, T>(builder.Client, () => new DeleteByKeyCommand<T>(key, version));
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

        #endregion

        #region InStore

        public static CommandBuilder<InStoreCommand<T>, T> InStore<T>(
            this DomainCommandBuilder<T> builder,
            string storeKey)
            where T : Resource<T>, IInStoreUsable
        {
            return new CommandBuilder<InStoreCommand<T>, T>(builder.Client, () => new InStoreCommand<T>(storeKey, null));
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
            return new CommandBuilder<InStoreCommand<SignInResult<T>>, SignInResult<T>>(builder.Client, () => builder.Build().InStore(storeKey));
        }

        public static CommandBuilder<InStoreCommand<SignInResult<T>>, SignInResult<T>> InStore<T>(
            this CommandBuilder<LoginCommand<T>, SignInResult<T>> builder,
            string storeKey)
            where T : Resource<T>, IInStoreUsable
        {
            return new CommandBuilder<InStoreCommand<SignInResult<T>>, SignInResult<T>>(builder.Client, () => builder.Build().InStore(storeKey));
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
            return new CommandBuilder<InStoreCommand<Token<T>>, Token<T>>(builder.Client, () => builder.Build().InStore(storeKey));
        }

        public static CommandBuilder<InStoreCommand<Token<T>>, Token<T>> InStore<T>(
            this CommandBuilder<CreateTokenForEmailVerificationCommand<T>, Token<T>> builder,
            string storeKey)
            where T : Resource<T>, IInStoreUsable
        {
            return new CommandBuilder<InStoreCommand<Token<T>>, Token<T>>(builder.Client, () => builder.Build().InStore(storeKey));
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
            return new CommandBuilder<InStoreCommand<PagedQueryResult<T>>, PagedQueryResult<T>>(builder.Client, () => builder.Build().InStore(storeKey));
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
            this QueryCommandBuilder<T> builder, Expression<Func<T, IComparable>> expression, SortDirection sortDirection = SortDirection.Ascending)
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

        //        public static UpdateCommandBuilder<UpdateCommand<Customer>, Customer> AddAddress(this UpdateCommandBuilder<UpdateCommand<Customer>, Customer> builder, Func<AddAddressUpdateAction, UpdateAction<Customer>> action)
        //        {
        //            return builder.AddAction(action(new AddAddressUpdateAction()));
        //        }

        #endregion

        #region Customers

        public static CommandBuilder<SignUpCommand<T>, SignInResult<T>> SignUp<T>(
            this DomainCommandBuilder<T> builder,
            CustomerDraft draft)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<SignUpCommand<T>, SignInResult<T>>(builder.Client, () => new SignUpCustomerCommand(draft) as SignUpCommand<T>);
        }

        public static CommandBuilder<LoginCommand<T>, SignInResult<T>> Login<T>(
            this DomainCommandBuilder<T> builder,
            string email,
            string password)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<LoginCommand<T>, SignInResult<T>>(builder.Client, () => new LoginCustomerCommand(email, password) as LoginCommand<T>);
        }

        public static CommandBuilder<ChangePasswordCommand<T>, T> ChangePassword<T>(
            this DomainCommandBuilder<T> builder,
            string id,
            int version,
            string currentPassword,
            string newPassword)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<ChangePasswordCommand<T>, T>(builder.Client, () => new ChangeCustomerPasswordCommand(id, version, currentPassword, newPassword) as ChangePasswordCommand<T>);
        }

        public static CommandBuilder<CreateTokenForPasswordResetCommand<T>, Token<T>> CreateTokenForPasswordResetting<T>(
            this DomainCommandBuilder<T> builder,
            string email,
            int? timeToLiveMinutes = null)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<CreateTokenForPasswordResetCommand<T>, Token<T>>(builder.Client, () => new CreateTokenForCustomerPasswordResetCommand(email, timeToLiveMinutes) as CreateTokenForPasswordResetCommand<T>);
        }

        public static CommandBuilder<CreateTokenForEmailVerificationCommand<T>, Token<T>> CreateTokenForEmailVerification<T>(
            this DomainCommandBuilder<T> builder,
            string id,
            int timeToLiveMinutes,
            int? version = null)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<CreateTokenForEmailVerificationCommand<T>, Token<T>>(
                builder.Client,
                () => new CreateTokenForCustomerEmailVerificationCommand(id, timeToLiveMinutes, version) as CreateTokenForEmailVerificationCommand<T>);
        }

        public static CommandBuilder<GetCommand<T>, T> GetByEmailToken<T>(
            this DomainCommandBuilder<T> builder,
            string emailToken)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<GetCommand<T>, T>(builder.Client, () => new GetCustomerByEmailTokenCommand(emailToken) as GetCommand<T>);
        }

        public static CommandBuilder<GetCommand<T>, T> GetByPasswordToken<T>(
            this DomainCommandBuilder<T> builder,
            string passwordToken)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<GetCommand<T>, T>(builder.Client, () => new GetCustomerByPasswordTokenCommand(passwordToken) as GetCommand<T>);
        }

        public static CommandBuilder<GetCommand<T>, T> GetByPasswordToken<T>(
            this CommandBuilder<CreateTokenForPasswordResetCommand<T>, Token<T>> builder)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<GetCommand<T>, T>(builder.Client, () =>
            {
                //builder.Build().
                var passwordToken = "";
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
            return new CommandBuilder<ResetPasswordCommand<T>, T>(builder.Client, () => new ResetCustomerPasswordCommand(tokenValue, newPassword, version) as ResetPasswordCommand<T>);
        }

        public static CommandBuilder<VerifyEmailCommand<T>, T> VerifyEmail<T>(
            this DomainCommandBuilder<T> builder,
            string tokenValue)
            where T : Resource<T>, ISignupable
        {
            return new CommandBuilder<VerifyEmailCommand<T>, T>(builder.Client, () => new VerifyCustomerEmailCommand(tokenValue) as VerifyEmailCommand<T>);
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

        #endregion
    }
}