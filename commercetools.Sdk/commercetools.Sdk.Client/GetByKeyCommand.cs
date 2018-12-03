namespace commercetools.Sdk.Client
{
    using commercetools.Sdk.Domain;
    using System.Collections.Generic;

    public class GetByKeyCommand<T> : GetCommand<T>
    {
        public override System.Type ResourceType => typeof(T);

        public GetByKeyCommand(string key)
        {
            Init(key);
        }

        public GetByKeyCommand(string key, List<Expansion<T>> expand) : base(expand)
        {
            Init(key);
        }

        private void Init(string key)
        {
            this.ParameterKey = Parameters.KEY;
            this.ParameterValue = key;
        }
    }
}