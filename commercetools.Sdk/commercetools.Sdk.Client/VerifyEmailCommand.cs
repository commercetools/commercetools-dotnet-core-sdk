namespace commercetools.Sdk.Client
{
    public abstract class VerifyEmailCommand<T> : Command<T>
    {
        public VerifyEmailCommand(string tokenValue)
        {
            this.TokenValue = tokenValue;
        }

        public VerifyEmailCommand(string tokenValue, int version)
        {
            this.TokenValue = tokenValue;
            this.Version = version;
        }

        public string TokenValue { get; private set; }

        public int? Version { get; private set; }

        public override System.Type ResourceType => typeof(T);
    }
}
