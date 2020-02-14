using System.Collections.Generic;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.Domain.ShippingMethods;

namespace commercetools.Sdk.Client
{
    /// <summary>
    /// Retrieves all the shipping methods that can ship to the given Location for an OrderEdit
    /// </summary>
    public class GetShippingMethodsForOrderEditCommand : GetMatchingQueryCommand<ShippingMethod>
    {
        public GetShippingMethodsForOrderEditCommand(string orderEditId, string country, string state = null)
        {
            this.Init(orderEditId, country, state);
        }

        private void Init(string orderEditId, string country, string state = null)
        {
            this.AdditionalParameters = new GetShippingMethodsForOrderEditAdditionalParameters
            {
                Country = country,
                State = state,
                OrderEditId = orderEditId
            };
        }
    }
}
