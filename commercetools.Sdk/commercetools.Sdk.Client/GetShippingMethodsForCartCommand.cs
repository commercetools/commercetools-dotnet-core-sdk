using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.Domain.ShippingMethods;

namespace commercetools.Sdk.Client
{
    /// <summary>
    /// Retrieves all the shipping methods that can ship to the shipping address of the given cart
    /// </summary>
    public class GetShippingMethodsForCartCommand : GetMatchingQueryCommand<ShippingMethod>
    {
        public override string UrlSuffix => "/matching-cart";

        public GetShippingMethodsForCartCommand(string cartId)
        {
            this.Init(cartId);
        }

        public GetShippingMethodsForCartCommand(string cartId, List<Expansion<ShippingMethod>> expand)
        {
            this.Init(cartId, expand);
        }

        private void Init(string cartId, List<Expansion<ShippingMethod>> expandPredicates = null)
        {
            this.AdditionalParameters = new GetShippingMethodsForCartAdditionalParameters
            {
                CartId = cartId
            };
            this.SetExpand(expandPredicates);
        }
    }
}
