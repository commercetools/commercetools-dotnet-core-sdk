using System;
using System.Linq.Expressions;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ProductProjections;
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
    }
}
