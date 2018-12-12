using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public abstract class GetMatchingCommand<T> : Command<T>
    {
        protected GetMatchingCommand(IGetMatchingParameters<T> parameters)
        {
            this.Parameters = parameters;
        }

        public IGetMatchingParameters<T> Parameters { get; }

        public override System.Type ResourceType => typeof(T);
    }
}