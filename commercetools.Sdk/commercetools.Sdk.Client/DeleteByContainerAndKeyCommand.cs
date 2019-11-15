using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    public class DeleteByContainerAndKeyCommand<T> : DeleteCommand<T>
    {
        public DeleteByContainerAndKeyCommand(string container, string key)
        {
            this.Init(container, key);
        }

        public DeleteByContainerAndKeyCommand(string container, string key, List<Expansion<T>> expand)
            : base(expand)
        {
            this.Init(container, key);
        }

        public string Container { get; set; }

        public string Key { get; set; }

        private void Init(string container, string key)
        {
            this.ParameterKey = null;
            this.Container = container;
            this.Key = key;
        }
    }
}
