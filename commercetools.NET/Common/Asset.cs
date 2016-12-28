using System.Collections.Generic;

using Newtonsoft.Json;

namespace commercetools.Common
{
    /// <summary>
    /// An Asset can be used to represent media assets, such as images, videos or PDFs.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-types.html#asset"/>
    public class Asset
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "sources")]
        public List<AssetSource> Sources { get; private set; }

        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; private set; }

        [JsonProperty(PropertyName = "description")]
        public LocalizedString Description { get; private set; }

        [JsonProperty(PropertyName = "tags")]
        public List<string> Tags { get; private set; }

        [JsonProperty(PropertyName = "custom")]
        public CustomFields.CustomFields Custom { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Asset(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Id = data.id;
            this.Sources = Helper.GetListFromJsonArray<AssetSource>(data.sources);
            this.Name = new LocalizedString(data.name);
            this.Description = new LocalizedString(data.description);
            this.Tags = Helper.GetStringListFromJsonArray(data.tags);
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
            Asset asset = obj as Asset;

            if (asset == null)
            {
                return false;
            }

            return (asset.Id.Equals(this.Id));
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
