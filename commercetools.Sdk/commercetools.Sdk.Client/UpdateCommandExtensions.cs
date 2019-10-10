using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.ProductProjections;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    public static class UpdateCommandExtensions
    {
        public static UpdateCommand<T> Expand<T>(this UpdateCommand<T> command, Expression<Func<T, Reference>> expression)
        {
            command.Expand.Add(new Expansion<T>(expression).ToString());

            return command;
        }

        public static UpdateCommand<T> Expand<T>(this UpdateCommand<T> command, string expression)
        {
            command.Expand.Add(expression);

            return command;
        }

        public static UpdateByIdCommand<T> UpdateById<T>(this IVersioned<T> resource, Func<List<UpdateAction<T>>, List<UpdateAction<T>>> updateBuilder)
            where T : Resource<T>
        {
            var actions = updateBuilder(new List<UpdateAction<T>>());
            return new UpdateByIdCommand<T>(resource, actions);
        }

        public static UpdateByKeyCommand<T> UpdateByKey<T>(this T resource, Func<List<UpdateAction<T>>, List<UpdateAction<T>>> updateBuilder)
            where T : Resource<T>, IKeyReferencable<T>, IVersioned<T>
        {
            var actions = updateBuilder(new List<UpdateAction<T>>());
            return new UpdateByKeyCommand<T>(resource.Key, resource.Version, actions);
        }

        public static List<UpdateAction<T>> AddUpdate<T>(this List<UpdateAction<T>> updateActions, UpdateAction<T> updateAction)
            where T : Resource<T>
        {
            updateActions.Add(updateAction);
            return updateActions;
        }
    }
}
