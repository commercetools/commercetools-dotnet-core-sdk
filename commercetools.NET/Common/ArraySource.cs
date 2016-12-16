using Newtonsoft.Json;

namespace commercetools.Common
{
    /// <summary>
    /// An AssetSource is a representation of an Asset in a specific format, e.g. a video in a certain encoding, or an image in a certain resolution.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-types.html#assetsource"/>
    public class ArraySource
    {
        #region Properties

        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "dimensions")]
        public AssetDimensions Dimensions { get; set; }

        [JsonProperty(PropertyName = "contentType")]
        public string ContentType { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public ArraySource() 
        {
        }

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ArraySource(dynamic data = null)
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

        #region Public Methods

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            ArraySource arraySource = obj as ArraySource;

            if (arraySource == null)
            {
                return false;
            }

            return object.Equals(arraySource.Uri, this.Uri);
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Uri.GetHashCode();
        }

        #endregion
    }
}
