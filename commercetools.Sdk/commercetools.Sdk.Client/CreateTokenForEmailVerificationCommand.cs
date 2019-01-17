using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public abstract class CreateTokenForEmailVerificationCommand<T> : Command<Token<T>>
    {
        public CreateTokenForEmailVerificationCommand(string id, int timeToLiveMinutes)
        {
            this.Id = id;
            this.TimeToLiveMinutes = timeToLiveMinutes;
        }

        public CreateTokenForEmailVerificationCommand(string id, int timeToLiveMinutes, int version)
        {
            this.Id = id;
            this.TimeToLiveMinutes = timeToLiveMinutes;
            this.Version = version;
        }

        public string Id { get; private set; }

        public int? Version { get; private set; }

        public int TimeToLiveMinutes { get; private set; }

        public override System.Type ResourceType => typeof(T);
    }
}
