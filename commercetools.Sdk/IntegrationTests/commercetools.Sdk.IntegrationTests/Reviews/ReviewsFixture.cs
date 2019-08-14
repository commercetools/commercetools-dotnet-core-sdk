using System;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Reviews;
using static commercetools.Sdk.IntegrationTests.GenericFixture;
using Type = commercetools.Sdk.Domain.Type;

namespace commercetools.Sdk.IntegrationTests.Reviews
{
    public static class ReviewsFixture
    {
        #region DraftBuilds
        public static ReviewDraft DefaultReviewDraft(ReviewDraft reviewDraft)
        {
            var random = TestingUtility.RandomInt();
            reviewDraft.Title = $"ReviewTitle_{random}";
            reviewDraft.Text = $"ReviewText_{random}";
            reviewDraft.Rating = random;
            reviewDraft.Key = TestingUtility.RandomString();
            return reviewDraft;
        }
        public static ReviewDraft DefaultReviewDraftWithKey(ReviewDraft draft, string key)
        {
            var reviewDraft = DefaultReviewDraft(draft);
            reviewDraft.Key = key;
            return reviewDraft;
        }
        public static ReviewDraft DefaultReviewDraftWithTarget(ReviewDraft draft, IReference target)
        {
            var reviewDraft = DefaultReviewDraft(draft);
            reviewDraft.Target = target;
            return reviewDraft;
        }
        public static ReviewDraft DefaultReviewDraftWithCustomType(ReviewDraft draft, Type type, Fields fields)
        {
            var customFieldsDraft = new CustomFieldsDraft
            {
                Type = type.ToKeyResourceIdentifier(),
                Fields = fields
            };

            var reviewDraft = DefaultReviewDraft(draft);
            reviewDraft.Custom = customFieldsDraft;

            return reviewDraft;
        }
        #endregion

        #region WithReview

        public static async Task WithReview( IClient client, Action<Review> func)
        {
            await With(client, new ReviewDraft(), DefaultReviewDraft, func);
        }
        public static async Task WithReview( IClient client, Func<ReviewDraft, ReviewDraft> draftAction, Action<Review> func)
        {
            await With(client, new ReviewDraft(), draftAction, func);
        }

        public static async Task WithReview( IClient client, Func<Review, Task> func)
        {
            await WithAsync(client, new ReviewDraft(), DefaultReviewDraft, func);
        }
        public static async Task WithReview( IClient client, Func<ReviewDraft, ReviewDraft> draftAction, Func<Review, Task> func)
        {
            await WithAsync(client, new ReviewDraft(), draftAction, func);
        }
        #endregion

        #region WithUpdateableReview

        public static async Task WithUpdateableReview(IClient client, Func<Review, Review> func)
        {
            await WithUpdateable(client, new ReviewDraft(), DefaultReviewDraft, func);
        }

        public static async Task WithUpdateableReview(IClient client, Func<ReviewDraft, ReviewDraft> draftAction, Func<Review, Review> func)
        {
            await WithUpdateable(client, new ReviewDraft(), draftAction, func);
        }

        public static async Task WithUpdateableReview(IClient client, Func<Review, Task<Review>> func)
        {
            await WithUpdateableAsync(client, new ReviewDraft(), DefaultReviewDraft, func);
        }
        public static async Task WithUpdateableReview(IClient client, Func<ReviewDraft, ReviewDraft> draftAction, Func<Review, Task<Review>> func)
        {
            await WithUpdateableAsync(client, new ReviewDraft(), draftAction, func);
        }

        #endregion
    }
}
