using System;
using System.IO;

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
        public string Url { get; private set; }

        [JsonProperty(PropertyName = "width")]
        public int? Width { get; private set; }

        [JsonProperty(PropertyName = "height")]
        public int? Height { get; private set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; private set; }

        [JsonProperty(PropertyName = "thumbUrl")]
        public string ThumbUrl { get; private set; }

        [JsonProperty(PropertyName = "smallUrl")]
        public string SmallUrl { get; private set; }

        [JsonProperty(PropertyName = "mediumUrl")]
        public string MediumUrl { get; private set; }

        [JsonProperty(PropertyName = "largeUrl")]
        public string LargeUrl { get; private set; }

        [JsonProperty(PropertyName = "zoomUrl")]
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
            this.Width = data.dimensions != null ? data.dimensions.w : null;
            this.Height = data.dimensions != null ? data.dimensions.h : null;
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

            return image.Url.Equals(this.Url);
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Url.GetHashCode();
        }  

        #endregion
    }
}
