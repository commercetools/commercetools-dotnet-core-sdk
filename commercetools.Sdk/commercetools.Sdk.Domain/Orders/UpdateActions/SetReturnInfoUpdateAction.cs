using System.Collections.Generic;
using commercetools.Sdk.Domain.Stores;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetReturnInfoUpdateAction : OrderUpdateAction
    {
        public override string Action => "setReturnInfo";
        public List<ReturnInfoDraft> Items{ get; set; }
    }
}