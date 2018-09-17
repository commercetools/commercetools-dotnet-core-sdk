using System.Net.Http;

namespace commercetools.Sdk.Client
{
    public class GetByKeyCommand : ICommand
    {
        public string Key { get; set; }

        public GetByKeyCommand(string key)
        {
            this.Key = key;
        }
    }
}