using System;
using System.IO;
using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.Products
{
    /// <summary>
    /// ImageDimensions
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#images"/>
    public class ImageDimensions
    {
        #region Properties

        [JsonProperty(PropertyName = "w")]
        public int Width { get; set; }

        [JsonProperty(PropertyName = "h")]
        public int Height { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Fills the object with data from an API response.
        /// </summary>
        /// <param name="data">API response</param>
        public ImageDimensions(dynamic data)
        {
            if (data == null)
            {
                return;
            }
            this.Width = data.w;
            this.Height = data.h;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public ImageDimensions(int width, int height)
        {
            this.Width = width;
            this.Height = height;
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
            ImageDimensions imageDimensions = obj as ImageDimensions;

            if (imageDimensions == null)
            {
                return false;
            }

            return imageDimensions.Width.Equals(this.Width) && imageDimensions.Height.Equals(this.Height);
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Of(this.Width).And(this.Height);
        }

        #endregion
    }
}
