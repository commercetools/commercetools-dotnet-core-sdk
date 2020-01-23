using System;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.OrderEdits;

namespace commercetools.Sdk.Client
{
    public class ApplyOrderEditCommand : ApplyResourceEditCommand<OrderEdit>
    {
        public ApplyOrderEditCommand(string id, int editVersion, int resourceVersion)
            : base(id, editVersion, resourceVersion)
        {
        }

        public ApplyOrderEditCommand(IVersioned<OrderEdit> orderEdit, int resourceVersion)
            : base(orderEdit.Id, orderEdit.Version, resourceVersion)
        {
        }
    }
}