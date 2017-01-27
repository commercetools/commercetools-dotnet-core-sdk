namespace commercetools.Types
{
    /// <summary>
    /// LocalizedStringType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-types.html#localizedstringtype"/>
    public class LocalizedStringType : FieldType
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalizedStringType()
            : base()
        {
            this.Name = "LocalizedString";
        }

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public LocalizedStringType(dynamic data)
            : base((object)data)
        {
        }

        #endregion
    }
}
