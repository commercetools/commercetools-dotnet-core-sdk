namespace commercetools.Types
{
    /// <summary>
    /// NumberType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-types.html#numbertype"/>
    public class NumberType : FieldType
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public NumberType()
            : base()
        {
            this.Name = "Number";
        }

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public NumberType(dynamic data)
            : base((object)data)
        {
        }

        #endregion
    }
}
