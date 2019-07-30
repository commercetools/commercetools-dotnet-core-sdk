using System.Collections.Generic;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.Domain.ShippingMethods;

namespace commercetools.Sdk.Client
{
    /// <summary>
    /// Get Shipping Methods for Location Command
    /// </summary>
    public class GetShippingMethodsForLocationCommand : GetCommand<ShippingMethod>
    {
        public GetShippingMethodsForLocationCommand(string country, string state = null, string currency = null, List<Expansion<ShippingMethod>> expand = null)
            : base(expand)
        {
            this.Init(country, state, currency);
        }

        private void Init(string country, string state = null, string currency = null)
        {
            this.ParameterKey = null;
            this.AdditionalParameters = new GetShippingMethodsForLocationAdditionalParameters
            {
                Country = country,
                State = state,
                Currency = currency
            };
        }
    }
}
