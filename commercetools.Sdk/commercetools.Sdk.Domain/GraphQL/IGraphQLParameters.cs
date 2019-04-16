using System.Collections.Generic;

namespace commercetools.Sdk.Domain.GraphQL
{
    public interface IGraphQLParameters
    {
        string Query { get; set; }
        Dictionary<string, object> Variables { get; set; }
        string OperationName { get; set; }
    }
}