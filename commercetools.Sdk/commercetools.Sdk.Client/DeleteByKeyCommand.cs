using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Client
{
    public class DeleteByKeyCommand<T> : DeleteCommand<T>
    {
        public DeleteByKeyCommand(string key, int version)
        {
            this.ParameterKey = Parameters.KEY;
            this.ParameterValue = key;
            this.Version = version;
        }
    }
}
