using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Reviews;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Reviews
{
    public class ReviewFixture : ClientFixture, IDisposable
    {
        public List<Review> ReviewsToDelete { get; private set; }

        public ReviewFixture(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
            this.ReviewsToDelete = new List<Review>();
        }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.ReviewsToDelete.Reverse();
            foreach (var review in this.ReviewsToDelete)
            {
                var deletedType = this.TryDeleteResource(review).Result;
            }
        }


        public Review CreateReview(ReviewDraft reviewDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            var review = commerceToolsClient.ExecuteAsync(new CreateCommand<Review>(reviewDraft)).Result;
            return review;
        }

    }
}
