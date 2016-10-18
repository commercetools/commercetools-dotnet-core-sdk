namespace commercetools.Types
{
    /// <summary>
    /// BooleanType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-types.html#booleantype"/>
    public class BooleanType : FieldType
    {
        #region Constructors

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public BooleanType(dynamic data = null)
            : base((object)data)
        {
        }

        #endregion
    }
}