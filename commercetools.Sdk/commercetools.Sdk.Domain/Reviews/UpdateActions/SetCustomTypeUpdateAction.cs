using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.Reviews
{
    public class SetCustomTypeUpdateAction : UpdateAction<Review>
    {
        public string Action => "setCustomType";
        public IReference<Type> Type { get; set; }
        public Fields Fields { get; set; }
    }
}
