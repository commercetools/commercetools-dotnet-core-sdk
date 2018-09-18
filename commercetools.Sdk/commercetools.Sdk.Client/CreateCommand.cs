using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Client
{
    public class CreateCommand<T> : ICommand<T>
    {
        public IDraft<T> Entity { get; set; }

        public CreateCommand(IDraft<T> entity)
        {
            this.Entity = entity;
        }
    }
}
