namespace commercetools.Sdk.Client
{
    public abstract class ChangePasswordCommand<T> : Command<T>
    {
        public ChangePasswordCommand(string id, int version, string currentPassword, string newPassword)
        {
            this.Id = id;
            this.Version = version;
            this.CurrentPassword = currentPassword;
            this.NewPassword = newPassword;
        }

        public override System.Type ResourceType => typeof(T);

        public string Id { get; private set; }

        public int Version { get; private set; }

        public string CurrentPassword { get; private set; }

        public string NewPassword { get; private set; }
    }
}