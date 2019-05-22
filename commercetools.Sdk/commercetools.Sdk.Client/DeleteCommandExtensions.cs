using System;
using System.Linq.Expressions;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ProductProjections;
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
    }
}
