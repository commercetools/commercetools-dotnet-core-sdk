using commercetools.Sdk.Domain;
using System.Net.Http;

namespace commercetools.Sdk.Client
{
    public class GetByCustomerIdCommand<T> : Command<T> where T : Cart
    {
        public string Key { get; set; }

        public GetByCustomerIdCommand(string key)
        {
            this.Key = key;
        }
    }
}