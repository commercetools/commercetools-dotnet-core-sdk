using Type = System.Type;

namespace commercetools.Sdk.HttpApi
{
    public interface IParametersBuilder
    {
        bool CanBuild(Type type);
    }
}
