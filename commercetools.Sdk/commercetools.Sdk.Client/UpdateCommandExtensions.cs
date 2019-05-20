using System;
using System.Linq.Expressions;
using commercetools.Sdk.Domain;
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
    }
}
