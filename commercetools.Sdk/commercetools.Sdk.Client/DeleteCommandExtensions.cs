using System;
using System.Linq.Expressions;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    public static class DeleteCommandExtensions
    {
        public static DeleteCommand<T> Expand<T>(this DeleteCommand<T> command, Expression<Func<T, Reference>> expression)
        {
            command.Expand.Add(new Expansion<T>(expression).ToString());

            return command;
        }

        public static DeleteCommand<T> Expand<T>(this DeleteCommand<T> command, string expression)
        {
            command.Expand.Add(expression);

            return command;
        }

        public static DeleteCommand<T> SetAdditionalParameters<T>(this DeleteCommand<T> command, IAdditionalParameters<T> additionalParameters)
        {
            command.AdditionalParameters = additionalParameters;
            return command;
        }

        public static DeleteCommand<T> DeleteById<T>(this IVersioned<T> resource)
            where T : Resource<T>
        {
            return new DeleteByIdCommand<T>(resource);
        }

        public static DeleteCommand<T> DeleteByKey<T>(this T resource)
            where T : Resource<T>, IVersioned<T>, IKeyReferencable<T>
        {
            return new DeleteByKeyCommand<T>(resource.Key, resource.Version);
        }
    }
}
