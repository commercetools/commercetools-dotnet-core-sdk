using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Client
{
    public class DeleteByKeyCommand<T> : ICommand<T>
    {
        public string Key { get; set; }
        public int Version { get; set; }

        public DeleteByKeyCommand(string key, int version)
        {
            this.Key = key;
            this.Version = version;
        }
    }
}
