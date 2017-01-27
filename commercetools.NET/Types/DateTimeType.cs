namespace commercetools.Types
{
    /// <summary>
    /// DateTimeType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-types.html#datetimetype"/>
    public class DateTimeType : FieldType
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public DateTimeType()
            : base()
        {
            this.Name = "DateTime";
        }

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public DateTimeType(dynamic data = null)
            : base((object)data)
        {
        }

        #endregion
    }
}
