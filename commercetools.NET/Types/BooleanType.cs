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
        /// Constructor.
        /// </summary>
        public BooleanType()
            : base()
        {
            this.Name = "Boolean";
        }

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public BooleanType(dynamic data)
            : base((object)data)
        {
        }

        #endregion
    }
}
