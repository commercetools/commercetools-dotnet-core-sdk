using commercetools.Categories;

using Newtonsoft.Json;

namespace commercetools.Messages
{
    /// <summary>
    /// This message is the result of the create category request. 
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-messages.html#categorycreated-message"/>
    public class CategoryCreatedMessage : Message
    {
        #region Properties

        [JsonProperty(PropertyName = "category")]
        public Category Category { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public CategoryCreatedMessage(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Category = new Category(data.category);
        }

        #endregion
    }
}
