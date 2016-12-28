using System;

using commercetools.Common;
using commercetools.CustomFields;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Reviews
{
    /// <summary>
    /// Reviews are used to evaluate products and channels.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-reviews.html#review"/>
    public class Review
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "version")]
        public int Version { get; private set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime? CreatedAt { get; private set; }

        [JsonProperty(PropertyName = "lastModifiedAt")]
        public DateTime? LastModifiedAt { get; private set; }

        [JsonProperty(PropertyName = "key")]
        public string Key { get; private set; }

        [JsonProperty(PropertyName = "uniquenessValue")]
        public string UniquenessValue { get; private set; }

        [JsonProperty(PropertyName = "locale")]
        public string Locale { get; private set; }

        [JsonProperty(PropertyName = "authorName")]
        public string AuthorName { get; private set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; private set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; private set; }

        [JsonProperty(PropertyName = "target")]
        public Reference Target { get; private set; }

        [JsonProperty(PropertyName = "rating")]
        public int? Rating { get; private set; }

        [JsonProperty(PropertyName = "state")]
        public Reference State { get; private set; }

        [JsonProperty(PropertyName = "includedInStatistics")]
        public bool? IncludedInStatistics { get; private set; }

        [JsonProperty(PropertyName = "customer")]
        public Reference Customer { get; private set; }

        [JsonProperty(PropertyName = "custom")]
        public CustomFields.CustomFields Custom { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Review(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Id = data.id;
            this.Version = data.version;
            this.CreatedAt = data.createdAt;
            this.LastModifiedAt = data.lastModifiedAt;
            this.Key = data.key;
            this.UniquenessValue = data.uniquenessValue;
            this.Locale = data.locale;
            this.AuthorName = data.authorName;
            this.Title = data.title;
            this.Text = data.text;
            this.Target = new Reference(data.target);
            this.Rating = data.rating;
            this.State = new Reference(data.state);
            this.IncludedInStatistics = data.includedInStatistics;
            this.Customer = new Reference(data.customer);
            this.Custom = new CustomFields.CustomFields(data.custom);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Review review = obj as Review;

            if (review == null)
            {
                return false;
            }

            return review.Id.Equals(this.Id);
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion
    }
}
