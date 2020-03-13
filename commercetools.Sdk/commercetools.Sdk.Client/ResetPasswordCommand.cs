namespace commercetools.Sdk.Client
{
    public abstract class ResetPasswordCommand<T> : Command<T>
    {
        public ResetPasswordCommand(string tokenValue, string newPassword, int? version = null)
        {
            this.Init(tokenValue, newPassword);
            this.Version = version;
        }

        public string NewPassword { get; private set; }

        public string TokenValue { get; private set; }

        public int? Version { get; private set; }

        public override System.Type ResourceType => typeof(T);

        private void Init(string tokenValue, string newPassword)
        {
            this.TokenValue = tokenValue;
            this.NewPassword = newPassword;
        }
    }
}
