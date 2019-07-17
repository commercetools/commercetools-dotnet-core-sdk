using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.DiscountCodes.DiscountCodesFixture;

namespace commercetools.Sdk.IntegrationTests.DiscountCodes
{
    [Collection("Integration Tests")]
    public class DiscountCodeIntegrationTests : IClassFixture<ServiceProviderFixture>
    {
        private readonly IClient client;

        public DiscountCodeIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateDiscountCode()
        {
            var code = $"CreateDiscountCode-{TestingUtility.RandomString()}";
            await WithDiscountCode(
                client, discountCodeDraft => DefaultDiscountCodeDraftWithCode(discountCodeDraft, code),
                discountCode => { Assert.Equal(code, discountCode.Code); });
        }

        [Fact]
        public async Task GetDiscountCodeById()
        {
            var code = $"GetDiscountCodeById-{TestingUtility.RandomString()}";
            await WithDiscountCode(
                client, discountCodeDraft => DefaultDiscountCodeDraftWithCode(discountCodeDraft, code),
                async discountCode =>
                {
                    var retrievedDiscountCode = await client
                        .ExecuteAsync(new GetByIdCommand<DiscountCode>(discountCode.Id));
                    Assert.Equal(code, retrievedDiscountCode.Code);
                });
        }


        [Fact]
        public async Task QueryDiscountCodes()
        {
            var code = $"QueryDiscountCodes-{TestingUtility.RandomString()}";
            await WithDiscountCode(
                client, discountCodeDraft => DefaultDiscountCodeDraftWithCode(discountCodeDraft, code),
                async discountCode =>
                {
                    var queryCommand = new QueryCommand<DiscountCode>();
                    queryCommand.Where(p => p.Code == discountCode.Code.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(code, returnedSet.Results[0].Code);
                });
        }

        [Fact]
        public async Task DeleteDiscountCodeById()
        {
            var code = $"DeleteDiscountCodeById-{TestingUtility.RandomString()}";
            await WithDiscountCode(
                client, discountCodeDraft => DefaultDiscountCodeDraftWithCode(discountCodeDraft, code),
                async discountCode =>
                {
                    await client.ExecuteAsync(new DeleteByIdCommand<DiscountCode>(discountCode));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<DiscountCode>(discountCode))
                    );
                });
        }


        #region UpdateActions


        #endregion
    }
}
