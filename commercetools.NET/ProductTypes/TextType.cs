namespace commercetools.ProductTypes
{
    /// <summary>
    /// TextType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#texttype"/>
    public class TextType : AttributeType
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextType()
            : base()
        {
        }

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public TextType(dynamic data)
            : base((object)data)
        {
        }

        #endregion
    }
}
