using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Reviews;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Reviews.ReviewsFixture;
using static commercetools.Sdk.IntegrationTests.Customers.CustomersFixture;
using static commercetools.Sdk.IntegrationTests.Projects.ProjectFixture;
using static commercetools.Sdk.IntegrationTests.States.StatesFixture;
using static commercetools.Sdk.IntegrationTests.Types.TypesFixture;
using static commercetools.Sdk.IntegrationTests.Products.ProductsFixture;

namespace commercetools.Sdk.IntegrationTests.Reviews
{
    [Collection("Integration Tests")]
    public class ReviewIntegrationTests
    {
        private readonly IClient client;

        public ReviewIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateReview()
        {
            var key = $"CreateReview-{TestingUtility.RandomString()}";
            await WithReview(
                client, reviewDraft => DefaultReviewDraftWithKey(reviewDraft, key),
                review => { Assert.Equal(key, (string) review.Key); });
        }

        [Fact]
        public async Task CreateReviewForProduct()
        {
            await WithProduct(client, async product =>
            {
                await WithReview(
                    client, reviewDraft => DefaultReviewDraftWithTarget(reviewDraft,
                        new ResourceIdentifier<Product> {Key = product.Key}),
                    review =>
                    {
                        Assert.NotNull(review.Target);
                        Assert.Equal(product.Id, review.Target.Id);
                    });
            });
        }

        [Fact]
        public async Task GetReviewById()
        {
            var key = $"GetReviewById-{TestingUtility.RandomString()}";
            await WithReview(
                client, reviewDraft => DefaultReviewDraftWithKey(reviewDraft, key),
                async review =>
                {
                    var retrievedReview = await client
                        .ExecuteAsync(new GetByIdCommand<Review>(review.Id));
                    Assert.Equal(key, retrievedReview.Key);
                });
        }

        [Fact]
        public async Task GetReviewByKey()
        {
            var key = $"GetReviewByKey-{TestingUtility.RandomString()}";
            await WithReview(
                client, reviewDraft => DefaultReviewDraftWithKey(reviewDraft, key),
                async review =>
                {
                    var retrievedReview = await client
                        .ExecuteAsync(new GetByKeyCommand<Review>(review.Key));
                    Assert.Equal(key, retrievedReview.Key);
                });
        }

        [Fact]
        public async Task QueryReviews()
        {
            var key = $"QueryReviews-{TestingUtility.RandomString()}";
            await WithReview(
                client, reviewDraft => DefaultReviewDraftWithKey(reviewDraft, key),
                async review =>
                {
                    var queryCommand = new QueryCommand<Review>();
                    queryCommand.Where(p => p.Key == review.Key.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(key, returnedSet.Results[0].Key);
                });
        }

        [Fact]
        public async Task DeleteReviewById()
        {
            var key = $"DeleteReviewById-{TestingUtility.RandomString()}";
            await WithReview(
                client, reviewDraft => DefaultReviewDraftWithKey(reviewDraft, key),
                async review =>
                {
                    await client.ExecuteAsync(new DeleteByIdCommand<Review>(review));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<Review>(review))
                    );
                });
        }

        [Fact]
        public async Task DeleteReviewByKey()
        {
            var key = $"DeleteReviewByKey-{TestingUtility.RandomString()}";
            await WithReview(
                client, reviewDraft => DefaultReviewDraftWithKey(reviewDraft, key),
                async review =>
                {
                    await client.ExecuteAsync(new DeleteByKeyCommand<Review>(review.Key, review.Version));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<Review>(review))
                    );
                });
        }


        #region UpdateActions

        [Fact]
        public async Task UpdateReviewSetKey()
        {
            var newKey = $"UpdateReviewSetKey-{TestingUtility.RandomString()}";
            await WithUpdateableReview(client, async review =>
            {
                var updateActions = new List<UpdateAction<Review>>();
                SetKeyUpdateAction setKeyAction = new SetKeyUpdateAction() {Key = newKey};
                updateActions.Add(setKeyAction);

                var updatedReview = await client
                    .ExecuteAsync(new UpdateByIdCommand<Review>(review, updateActions));

                Assert.Equal(newKey, updatedReview.Key);
                return updatedReview;
            });
        }

        [Fact]
        public async Task UpdateReviewSetAuthorName()
        {
            var key = $"UpdateReviewSetAuthorName-{TestingUtility.RandomString()}";
            var authorName = $"AuthorName-{TestingUtility.RandomString()}";
            await WithUpdateableReview(client, reviewDraft => DefaultReviewDraftWithKey(reviewDraft, key),
                 async review =>
                {
                    var updateActions = new List<UpdateAction<Review>>();
                    var setAuthorNameAction = new SetAuthorNameUpdateAction {AuthorName = authorName};
                    updateActions.Add(setAuthorNameAction);

                    var updatedReview = await client
                        .ExecuteAsync(new UpdateByKeyCommand<Review>(key, review.Version, updateActions));

                    Assert.Equal(authorName, updatedReview.AuthorName);
                    return updatedReview;
                });
        }

        [Fact]
        public async Task UpdateReviewSetCustomer()
        {
            var key = $"UpdateReviewSetCustomer-{TestingUtility.RandomString()}";

            await WithCustomer(client, async customer =>
            {
                await WithUpdateableReview(client, reviewDraft => DefaultReviewDraftWithKey(reviewDraft, key),
                    async review =>
                    {
                        var updateActions = new List<UpdateAction<Review>>();
                        var setCustomerAction = new SetCustomerUpdateAction
                        {
                            Customer = customer.ToKeyResourceIdentifier()
                        };
                        updateActions.Add(setCustomerAction);
                        var updateCommand = new UpdateByKeyCommand<Review>(key, review.Version, updateActions);
                        updateCommand.Expand(r => r.Customer);

                        var updatedReview = await client.ExecuteAsync(updateCommand);

                        Assert.NotNull(updatedReview.Customer.Obj);
                        Assert.Equal(customer.Key, updatedReview.Customer.Obj.Key);
                        return updatedReview;
                    });
            });
        }

        [Fact]
        public async Task UpdateReviewSetRating()
        {
            var newRating = TestingUtility.RandomInt(1, 100);
            await WithUpdateableReview(client,
                async review =>
                {
                    var updateActions = new List<UpdateAction<Review>>();
                    var setRatingAction = new SetRatingUpdateAction() {Rating = newRating};
                    updateActions.Add(setRatingAction);

                    var updatedReview = await client.ExecuteAsync(new UpdateByIdCommand<Review>(review, updateActions));
                    Assert.Equal(newRating, updatedReview.Rating);
                    return updatedReview;
                });
        }

        [Fact]
        public async Task UpdateReviewSetTarget()
        {
            await WithProduct(client, async product =>
            {
                await WithUpdateableReview(client, async review =>
                {
                    var updateActions = new List<UpdateAction<Review>>();
                    var setTargetAction = new SetTargetUpdateAction
                    {
                        Target = new ResourceIdentifier<Product>
                        {
                            Key = product.Key
                        }
                    };
                    updateActions.Add(setTargetAction);

                    var updatedReview = await client
                        .ExecuteAsync(new UpdateByIdCommand<Review>(review, updateActions));

                    Assert.NotNull(updatedReview.Rating);
                    Assert.Equal(product.Id, updatedReview.Target.Id);
                    return updatedReview;
                });
            });
        }

        [Fact]
        public async Task UpdateReviewSetText()
        {
            var newText = TestingUtility.RandomString();
            await WithUpdateableReview(client,
                async review =>
                {
                    var updateActions = new List<UpdateAction<Review>>();
                    var setTextAction = new SetTextUpdateAction {Text = newText};
                    updateActions.Add(setTextAction);

                    var updatedReview = await client.ExecuteAsync(new UpdateByIdCommand<Review>(review, updateActions));
                    Assert.Equal(newText, updatedReview.Text);
                    return updatedReview;
                });
        }

        [Fact]
        public async Task UpdateReviewSetTitle()
        {
            var newTitle = TestingUtility.RandomString();
            await WithUpdateableReview(client,
                 async review =>
                {
                    var updateActions = new List<UpdateAction<Review>>();
                    var setTextAction = new SetTitleUpdateAction {Title = newTitle};
                    updateActions.Add(setTextAction);

                    var updatedReview = await client.ExecuteAsync(new UpdateByIdCommand<Review>(review, updateActions));
                    Assert.Equal(newTitle, updatedReview.Title);
                    return updatedReview;
                });
        }

        [Fact]
        public async Task UpdateReviewSetLocale()
        {
            var projectLanguages = GetProjectLanguages(client);
            Assert.True(projectLanguages.Count > 0); //make sure that project has at least one language
            string locale = projectLanguages[0];

            await WithUpdateableReview(client,
                async review =>
                {
                    Assert.Null(review.Locale);

                    var updateActions = new List<UpdateAction<Review>>();
                    var setLocaleAction = new SetLocaleUpdateAction {Locale = locale};
                    updateActions.Add(setLocaleAction);

                    var updatedReview = await client.ExecuteAsync(new UpdateByIdCommand<Review>(review, updateActions));
                    Assert.Equal(locale, updatedReview.Locale);
                    return updatedReview;
                });
        }

        [Fact]
        public async Task UpdateReviewTransitionToNewState()
        {
            await WithState(client, stateDraft => DefaultStateDraftWithType(stateDraft, StateType.ReviewState),
                async state =>
                {
                    await WithUpdateableReview(client,
                        async review =>
                        {
                            var updateActions = new List<UpdateAction<Review>>();
                            var transitionStateAction = new TransitionStateUpdateAction
                            {
                                State = state.ToKeyResourceIdentifier()
                            };
                            updateActions.Add(transitionStateAction);

                            var updatedReview = await client.ExecuteAsync(new UpdateByIdCommand<Review>(review, updateActions));
                            Assert.NotNull(updatedReview.State);
                            Assert.Equal(state.Id, updatedReview.State.Id);
                            return updatedReview;
                        });
                });
        }

        [Fact]
        public async Task UpdateReviewSetCustomType()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithUpdateableReview(client,
                    async review =>
                    {
                        var updateActions = new List<UpdateAction<Review>>();
                        var setTypeAction = new SetCustomTypeUpdateAction
                        {
                            Type = type.ToKeyResourceIdentifier(),
                            Fields = fields
                        };
                        updateActions.Add(setTypeAction);

                        var updatedReview = await client.ExecuteAsync(new UpdateByIdCommand<Review>(review, updateActions));

                        Assert.Equal(type.Id, updatedReview.Custom.Type.Id);
                        return updatedReview;
                    });
            });
        }

        [Fact]
        public async Task UpdateReviewSetCustomField()
        {
            var fields = CreateNewFields();
            var newValue = TestingUtility.RandomString(10);

            await WithType(client, async type =>
            {
                await WithUpdateableReview(client,
                    reviewDraft => DefaultReviewDraftWithCustomType(reviewDraft, type, fields),
                    async review =>
                    {
                        Assert.Equal(type.Id, review.Custom.Type.Id);

                        var updateActions = new List<UpdateAction<Review>>();
                        var setCustomFieldUpdateAction = new SetCustomFieldUpdateAction()
                        {
                            Name = "string-field", Value = newValue
                        };
                        updateActions.Add(setCustomFieldUpdateAction);

                        var updatedReview = await client.ExecuteAsync(new UpdateByIdCommand<Review>(review, updateActions));

                        Assert.Equal(newValue, updatedReview.Custom.Fields["string-field"]);
                        return updatedReview;
                    });
            });
        }

        #endregion
    }
}
