using Newtonsoft.Json;

namespace commercetools.Common
{
    /// <summary>
    /// The width and height of the asset source.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-types.html#assetdimensions"/>
    public class AssetDimensions
    {
        #region Properties

        [JsonProperty(PropertyName = "w")]
        public int? W { get; set; }

        [JsonProperty(PropertyName = "h")]
        public int? H { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public AssetDimensions() 
        {
        }

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public AssetDimensions(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.W = data.w;
            this.H = data.h;
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
            AssetDimensions assetDimensions = obj as AssetDimensions;

            if (assetDimensions == null)
            {
                return false;
            }

            return object.Equals(assetDimensions.H, this.H) && object.Equals(assetDimensions.W, this.W);
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.H.GetHashCode() ^ this.W.GetHashCode();
        }

        #endregion
    }
}
