using Newtonsoft.Json;

namespace commercetools.Types
{
    /// <summary>
    /// SetType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-types.html#settype"/>
    public class SetType : FieldType
    {
        #region Properties

        [JsonProperty(PropertyName = "elementType")]
        public FieldType ElementType { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetType()
            : base()
        {
            this.Name = "Set";        
        }

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public SetType(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.ElementType = FieldTypeFactory.Create(data);
        }

        #endregion
    }
}
