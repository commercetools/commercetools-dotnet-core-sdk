using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Client
{
    public class CreateCommand : ICommand
    {
        public object Entity { get; set; }

        public CreateCommand(object entity)
        {
            this.Entity = entity;
        }
    }
}
