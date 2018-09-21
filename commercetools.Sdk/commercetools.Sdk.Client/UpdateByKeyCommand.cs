using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Client
{
    public class UpdateByIdCommand<T> : Command<T>
    {
        public Guid Guid { get; set; }
        public int Version { get; set; }
        public IList<UpdateAction> UpdateActions { get; set; }

        public UpdateByIdCommand(Guid guid, int version, IList<UpdateAction> updateActions)
        {
            this.Guid = guid;
            this.Version = version;
            this.UpdateActions = updateActions;
        }
    }
}
