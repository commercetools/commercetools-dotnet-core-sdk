using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ProductProjections;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    public class SearchProductProjectionsCommand : SearchCommand<ProductProjection>
    {
        public SearchProductProjectionsCommand(ISearchParameters<ProductProjection> searchParameters)
            : base(searchParameters)
        {
        }

        public SearchProductProjectionsCommand()
            : base(new ProductProjectionSearchParameters())
        {
        }

        public SearchProductProjectionsCommand(ISearchParameters<ProductProjection> searchParameters, IAdditionalParameters<ProductProjection> additionalParameters)
            : base(searchParameters, additionalParameters)
        {
        }

        public SearchProductProjectionsCommand(List<Expansion<ProductProjection>> expandPredicates, ISearchParameters<ProductProjection> searchParameters, IAdditionalParameters<ProductProjection> additionalParameters)
            : base(expandPredicates, searchParameters, additionalParameters)
        {
        }
    }
}
