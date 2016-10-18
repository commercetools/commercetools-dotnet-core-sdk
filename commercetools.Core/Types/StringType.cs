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
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public StringType(dynamic data = null)
            : base((object)data)
        {
        }

        #endregion
    }
}
