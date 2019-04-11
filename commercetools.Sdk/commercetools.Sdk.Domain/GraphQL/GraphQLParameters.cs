using System.Collections.Generic;

namespace commercetools.Sdk.Domain.GraphQL
{
    public class GraphQLParameters: IAdditionalParameters, IGraphQLParameters
    {
        public string Query { get; set; }
        public Dictionary<string, object> Variables { get; set; }
        public string OperationName { get; set; }

        public GraphQLParameters(string query, Dictionary<string, object> variables = null, string operationName = null)
        {
            this.Query = query;
            this.Variables = variables;
            this.OperationName = operationName;
        }
    }
}
