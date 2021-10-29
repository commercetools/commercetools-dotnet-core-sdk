using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Client
{
    public abstract class CheckCommand<T> : Command<T>
        where T : Resource<T>, ICheckable<T>
    {
        protected CheckCommand()
        {
        }

        public string ParameterKey { get; protected set; }

        public object ParameterValue { get; protected set; }

        public override System.Type ResourceType => typeof(T);
    }
}
