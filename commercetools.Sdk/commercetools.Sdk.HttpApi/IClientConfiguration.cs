using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public interface IClientConfiguration
    {
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string Scope { get; set; }
    }
}
