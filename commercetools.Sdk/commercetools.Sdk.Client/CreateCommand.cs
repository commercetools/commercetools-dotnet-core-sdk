using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Client
{
    public class CreateCommand<T> : Command<T>
    {
        public IDraft<T> Entity { get; private set; }

        public CreateCommand(IDraft<T> entity)
        {
            this.Entity = entity;
        }
    }
}
