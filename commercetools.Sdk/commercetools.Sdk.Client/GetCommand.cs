namespace commercetools.Sdk.Client
{
    // TODO Add expand
    public abstract class GetCommand<T> : Command<T>
    {
        public string ParameterKey { get; protected set; }
        public object ParameterValue { get; protected set; }
    }
}
