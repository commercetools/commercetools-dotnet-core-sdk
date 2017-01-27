namespace commercetools.Types
{
    /// <summary>
    /// StringType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-types.html#stringtype"/>
    public class StringType : FieldType
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public StringType()
            : base()
        {
            this.Name = "String";
        }

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public StringType(dynamic data)
            : base((object)data)
        {
        }

        #endregion
    }
}
