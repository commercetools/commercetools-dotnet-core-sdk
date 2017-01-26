namespace commercetools.Types
{
    /// <summary>
    /// DateType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-types.html#datetype"/>
    public class DateType : FieldType
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public DateType()
            : base()
        {
            this.Name = "Date";
        }

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public DateType(dynamic data)
            : base((object)data)
        {
        }

        #endregion
    }
}
