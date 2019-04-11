using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.GraphQL;
using Type = System.Type;

namespace commercetools.Sdk.Client
{
    public class GraphQLCommand<T> : Command<T>
    {
        public GraphQLCommand(IGraphQLParameters graphQlParameters)
        {
            this.GraphQlParameters = graphQlParameters;
        }

        public GraphQLCommand(IGraphQLParameters graphQlParameters, IAdditionalParameters<T> additionalParameters)
            : this(graphQlParameters)
        {
            this.AdditionalParameters = additionalParameters;
        }

        public override Type ResourceType => typeof(T);

        public IGraphQLParameters GraphQlParameters { get; }
    }
}
