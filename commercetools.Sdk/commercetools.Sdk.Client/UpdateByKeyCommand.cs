namespace commercetools.Sdk.Client
{
    using System.Collections.Generic;
    using Domain;

    public class UpdateByKeyCommand<T> : UpdateCommand<T>
    {
        public UpdateByKeyCommand(string key, int version, List<UpdateAction<T>> updateActions)
        {
            this.ParameterKey = Parameters.Key;
            this.ParameterValue = key;
            this.Version = version;
            this.UpdateActions.AddRange(updateActions);
        }

        public override System.Type ResourceType => typeof(T);
    }
}