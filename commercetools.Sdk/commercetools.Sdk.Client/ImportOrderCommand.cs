using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Client
{
    public class ImportOrderCommand : ImportCommand<Order>
    {
        public ImportOrderCommand(IImportDraft<Order> entity)
            : base(entity)
        {
        }

        public ImportOrderCommand(IImportDraft<Order> entity, IAdditionalParameters<Order> additionalParameters)
            : base(entity, additionalParameters)
        {
        }
    }
}