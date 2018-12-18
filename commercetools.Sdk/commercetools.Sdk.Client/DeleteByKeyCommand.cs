using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public class DeleteByKeyCommand<T> : DeleteCommand<T>
    {
        public DeleteByKeyCommand(string key, int version)
        {
            this.Init(key, version);
        }

        public DeleteByKeyCommand(string key, int version, IAdditionalParameters<T> additionalParameters)
        {
            this.Init(key, version);
            this.AdditionalParameters = additionalParameters;
        }

        private void Init(string key, int version)
        {
            this.ParameterKey = Parameters.Key;
            this.ParameterValue = key;
            this.Version = version;
        }
    }
}