using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Client
{
    public class UpdateByKeyCommand : ICommand
    {
        public string Key { get; set; }
        public int Version { get; set; }
        public IList<UpdateAction> UpdateActions { get; set; }

        public UpdateByKeyCommand(string key, int version, IList<UpdateAction> updateActions)
        {
            this.Key = key;
            this.Version = version;
            this.UpdateActions = updateActions;
        }
    }
}
