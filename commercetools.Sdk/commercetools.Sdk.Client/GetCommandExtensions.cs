using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    public static class GetCommandExtensions
    {
        public static GetCommand<T> Expand<T>(this GetCommand<T> command, Expression<Func<T, Reference>> expression)
        {
            command.Expand.Add(new Expansion<T>(expression).ToString());

            return command;
        }

        public static GetCommand<T> Expand<T>(this GetCommand<T> command, string expression)
        {
            command.Expand.Add(expression);

            return command;
        }

        public static GetCommand<T> SetAdditionalParameters<T>(this GetCommand<T> command, IAdditionalParameters<T> additionalParameters)
        {
            command.AdditionalParameters = additionalParameters;
            return command;
        }

        public static GetByIdCommand<T> GetById<T>(this IIdentifiable<T> resource)
            where T : Resource<T>
        {
            return new GetByIdCommand<T>(resource);
        }

        public static CheckByIdCommand<T> CheckById<T>(this IIdentifiable<T> resource)
            where T : Resource<T>, ICheckable<T>
        {
            return new CheckByIdCommand<T>(resource);
        }

        public static GetByKeyCommand<T> GetByKey<T>(this IKeyReferencable<T> resource)
            where T : Resource<T>
        {
            return new GetByKeyCommand<T>(resource.Key);
        }

        public static CheckByKeyCommand<T> CheckByKey<T>(this IKeyReferencable<T> resource)
            where T : Resource<T>, ICheckable<T>
        {
            return new CheckByKeyCommand<T>(resource.Key);
        }
    }
}
