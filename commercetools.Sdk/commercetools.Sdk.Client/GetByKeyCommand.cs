using System.Net.Http;

namespace commercetools.Sdk.Client
{
    public class GetByKeyCommand<T> : GetCommand<T>
    {
        public GetByKeyCommand(string key)
        {
            this.ParameterKey = "key";
            this.ParameterValue = key;
        }
    }
}