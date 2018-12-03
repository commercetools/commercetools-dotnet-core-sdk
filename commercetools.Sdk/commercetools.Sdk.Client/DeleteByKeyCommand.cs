﻿namespace commercetools.Sdk.Client
{
    public class DeleteByKeyCommand<T> : DeleteCommand<T>
    {
        public override System.Type ResourceType => typeof(T);

        public DeleteByKeyCommand(string key, int version)
        {
            this.ParameterKey = Parameters.KEY;
            this.ParameterValue = key;
            this.Version = version;
        }
    }
}