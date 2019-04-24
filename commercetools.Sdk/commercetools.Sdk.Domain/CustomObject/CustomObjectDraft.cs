using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.CustomObject
{
    public class CustomObjectDraft<T> : IDraft<CustomObject<T>>
    {
        public string Container { get; set; }

        public string Key { get; set; }

        public T Value { get; set; }

        public int? Version { get; set; }

    }
}
