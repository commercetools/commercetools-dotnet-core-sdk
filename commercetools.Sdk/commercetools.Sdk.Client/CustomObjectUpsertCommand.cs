using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CustomObjects;

namespace commercetools.Sdk.Client
{
    public class CustomObjectUpsertCommand<T> : UpsertCommand<CustomObject<T>>
    {
        public CustomObjectUpsertCommand(IDraft<CustomObject<T>> entity)
            : base(entity)
        {
        }
    }
}
