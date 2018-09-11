using commercetools.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public class GetByKeyCommand : IHttpApiCommand
    {
        private string key;

        public GetByKeyCommand(string key)
        {
            this.key = key;
        }

        public HttpMethod HttpMethod => HttpMethod.Get;

        public string RequestUriEnd => $"/key={this.key}";        
    }
}
