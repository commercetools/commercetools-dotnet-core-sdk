namespace commercetools.Sdk.Client
{
    public abstract class GetCommand<T> : Command<T>
    {
        public string ParameterKey { get; protected set; }
        public object ParameterValue { get; protected set; }
    }
}
