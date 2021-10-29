using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Client
{
    public class CheckByKeyCommand<T> : CheckCommand<T>
        where T : Resource<T>, ICheckable<T>
    {
        public CheckByKeyCommand(string key)
        {
            this.Init(key);
        }

        private void Init(string key)
        {
            this.ParameterKey = Parameters.Key;
            this.ParameterValue = key;
        }
    }
}