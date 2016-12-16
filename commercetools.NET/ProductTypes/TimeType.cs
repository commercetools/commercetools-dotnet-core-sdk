namespace commercetools.ProductTypes
{
    /// <summary>
    /// TimeType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#timetype"/>
    public class TimeType : AttributeType
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public TimeType()
            : base()
        {
        }

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public TimeType(dynamic data)
            : base((object)data)
        {
        }

        #endregion
    }
}
