using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Client
{
    public class DeleteByIdCommand<T> : Command<T>
    {
        public Guid Guid { get; set; }
        public int Version { get; set; }

        public DeleteByIdCommand(Guid guid, int version)
        {
            this.Guid = guid;
            this.Version = version;
        }
    }
}
