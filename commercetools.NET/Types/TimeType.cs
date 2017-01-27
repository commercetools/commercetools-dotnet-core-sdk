namespace commercetools.Types
{
    /// <summary>
    /// TimeType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-types.html#timetype"/>
    public class TimeType : FieldType
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public TimeType()
            : base()
        {
            this.Name = "Time";
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
