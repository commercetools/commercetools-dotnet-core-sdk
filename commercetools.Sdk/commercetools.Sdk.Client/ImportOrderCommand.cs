using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Client
{
    public abstract class ImportOrderCommand : ImportCommand<Order>
    {
        protected ImportOrderCommand(IImportDraft<Order> entity)
            : base(entity)
        {
        }
    }
}