using System.Collections.Generic;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.Domain.ShippingMethods;

namespace commercetools.Sdk.Client
{
    /// <summary>
    /// Get Shipping Methods for Location Command
    /// </summary>
    public class GetShippingMethodsForLocationCommand : GetMatchingQueryCommand<ShippingMethod>
    {
        public GetShippingMethodsForLocationCommand(
            string country,
            string state = null,
            string currency = null,
            IEnumerable<Expansion<ShippingMethod>> expand = null,
            IEnumerable<Sort<ShippingMethod>> sort = null)
        {
            this.AdditionalParameters = new GetShippingMethodsForLocationAdditionalParameters
            {
                Country = country,
                State = state,
                Currency = currency
            };
            this.SetExpand(expand);
            this.SetSort(sort);
        }

        public override string UrlSuffix => "/matching-location";
    }
}