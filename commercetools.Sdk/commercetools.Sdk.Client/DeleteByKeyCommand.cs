namespace commercetools.Sdk.Client
{
    public class DeleteByKeyCommand<T> : DeleteCommand<T>
    {
        public DeleteByKeyCommand(string key, int version)
        {
            this.ParameterKey = Parameters.Key;
            this.ParameterValue = key;
            this.Version = version;
        }

        public override System.Type ResourceType => typeof(T);
    }
}