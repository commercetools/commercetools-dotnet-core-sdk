using Newtonsoft.Json;

namespace commercetools.Common
{
    /// <summary>
    /// An AssetSource is a representation of an Asset in a specific format, e.g. a video in a certain encoding, or an image in a certain resolution.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-types.html#assetsource"/>
    public class AssetSource
    {
        #region Properties

        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; private set; }

        [JsonProperty(PropertyName = "key")]
        public string Key { get; private set; }

        [JsonProperty(PropertyName = "dimensions")]
        public AssetDimensions Dimensions { get; private set; }

        [JsonProperty(PropertyName = "contentType")]
        public string ContentType { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public AssetSource(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.Uri = data.uri;
            this.Key = data.key;
            this.Dimensions = new AssetDimensions(data.dimensions);
            this.ContentType = data.contentType;
        }

        #endregion
    }
}
