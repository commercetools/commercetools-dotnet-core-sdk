using System.Collections.Generic;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public class UpdateByKeyCommand<T> : UpdateCommand<T>
    {
        public UpdateByKeyCommand(string key, int version, List<UpdateAction<T>> updateActions)
        {
            this.ParameterKey = Parameters.Key;
            this.ParameterValue = key;
            this.Version = version;
            this.UpdateActions.AddRange(updateActions);
        }
    }
}