using System.Collections.Generic;

namespace commercetools.Sdk.Domain.DiscountCodes
{
    public class ChangeGroupsUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "changeGroups";
        public List<string> Groups { get; set; }
    }
}