using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public abstract class CreateTokenForPasswordResetCommand<T> : Command<Token<T>>
    {
        public CreateTokenForPasswordResetCommand(string email, int? timeToLiveMinutes = null)
        {
            this.Email = email;
            this.TimeToLiveMinutes = timeToLiveMinutes;
        }

        public string Email { get; private set; }

        public int? TimeToLiveMinutes { get; private set; }

        public override System.Type ResourceType => typeof(T);
    }
}
