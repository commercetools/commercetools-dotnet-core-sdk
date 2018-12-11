using System.Collections.Generic;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.HttpApi
{
    public class ParametersBuilderFactory<TBuilder> : IParametersBuilderFactory<TBuilder>
        where TBuilder : IParametersBuilder
    {
        private readonly IEnumerable<TBuilder> registeredBuilders;

        public ParametersBuilderFactory(IEnumerable<TBuilder> registeredBuilders)
        {
            this.registeredBuilders = registeredBuilders;
        }

        public TBuilder GetParameterBuilder(IParameters parameters)
        {
            foreach (IParametersBuilder registeredBuilder in this.registeredBuilders)
            {
                if (registeredBuilder.CanBuild(parameters.GetType()))
                {
                    return (TBuilder)registeredBuilder;
                }
            }

            return default(TBuilder);
        }
    }
}
