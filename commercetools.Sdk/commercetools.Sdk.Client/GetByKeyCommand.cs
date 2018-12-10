namespace commercetools.Sdk.Client
{
    using System.Collections.Generic;
    using Domain;

    public class GetByKeyCommand<T> : GetCommand<T>
    {
        public GetByKeyCommand(string key)
        {
            this.Init(key);
        }

        public GetByKeyCommand(string key, List<Expansion<T>> expand)
            : base(expand)
        {
            this.Init(key);
        }

        private void Init(string key)
        {
            this.ParameterKey = Parameters.Key;
            this.ParameterValue = key;
        }
    }
}