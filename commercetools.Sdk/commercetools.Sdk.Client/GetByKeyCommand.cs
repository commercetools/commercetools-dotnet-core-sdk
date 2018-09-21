using System.Net.Http;

namespace commercetools.Sdk.Client
{
    public class GetByKeyCommand<T> : Command<T>
    {
        public string Key { get; set; }

        public GetByKeyCommand(string key)
        {
            this.Key = key;
        }
    }
}