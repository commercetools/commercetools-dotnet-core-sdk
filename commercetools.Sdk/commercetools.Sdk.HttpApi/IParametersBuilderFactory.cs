using commercetools.Sdk.Domain;

namespace commercetools.Sdk.HttpApi
{
    public interface IParametersBuilderFactory<out TBuilder>
        where TBuilder : IParametersBuilder
    {
        TBuilder GetParameterBuilder(IParameters additionalParameters);
    }
}
