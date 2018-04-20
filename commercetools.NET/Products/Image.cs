using System;
using System.IO;
using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.Products
{
    /// <summary>
    /// Image
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#images"/>
    public class Image
    {
        #region Properties

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "dimensions")]
        public ImageDimensions Dimensions { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonIgnore]
        public int? Width {
            get
            {
                if (this.Dimensions != null)
                {
                    return this.Dimensions.Width;
                }
                return null; 
            }
        }

        [JsonIgnore]
        public int? Height {
            get
            {
                if (this.Dimensions != null)
                {
                    return this.Dimensions.Height;
                }
                return null;
            }
        }

        [JsonIgnore]
        public string ThumbUrl { get; private set; }

        [JsonIgnore]
        public string SmallUrl { get; private set; }

        [JsonIgnore]
        public string MediumUrl { get; private set; }

        [JsonIgnore]
        public string LargeUrl { get; private set; }

        [JsonIgnore]
        public string ZoomUrl { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Fills the object with data from an API response.
        /// </summary>
        /// <param name="data">API response</param>
        public Image(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Url = data.url;
            this.Dimensions = new ImageDimensions(data.dimensions);
            this.Label = data.label;

            Uri uri = new Uri(this.Url);

            if (Path.HasExtension(uri.AbsoluteUri))
            {
                string path = String.Format("{0}{1}{2}{3}", uri.Scheme, Uri.SchemeDelimiter, uri.Authority, uri.AbsolutePath);
                string extension = Path.GetExtension(path);

                this.ThumbUrl = this.Url.Replace(extension, string.Concat("-thumb", extension));
                this.SmallUrl = this.Url.Replace(extension, string.Concat("-small", extension));
                this.MediumUrl = this.Url.Replace(extension, string.Concat("-medium", extension));
                this.LargeUrl = this.Url.Replace(extension, string.Concat("-large", extension));
                this.ZoomUrl = this.Url.Replace(extension, string.Concat("-zoom", extension));
            }
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public Image(string url, ImageDimensions dimensions)
        {
            this.Url = url;
            this.Dimensions = dimensions;
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
            Image image = obj as Image;

            if (image == null)
            {
                return false;
            }

            return image.Url.Equals(this.Url) && image.Dimensions.Equals(this.Dimensions) && image.Label.Equals(this.Label);
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Of(this.Url).And(this.Dimensions).And(this.Label);
        }  

        #endregion
    }
}
