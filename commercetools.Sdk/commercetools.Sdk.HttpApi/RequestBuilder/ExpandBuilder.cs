using commercetools.Sdk.Domain;
using commercetools.Sdk.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public static class RequestBuilderExtensions
    {
        public static List<KeyValuePair<string, string>> GetQueryStringParameters<T>(this List<Expansion<T>> expansions, IExpansionExpressionVisitor expansionExpressionVisitor)
        {
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            foreach (var expansion in expansions)
            {
                string expandPath = expansionExpressionVisitor.GetPath(expansion.Expression);
                queryStringParameters.Add(new KeyValuePair<string, string>("expand", expandPath));
            }            
            return queryStringParameters;
        }
    }
}
