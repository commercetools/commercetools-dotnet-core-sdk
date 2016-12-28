using System.Collections.Generic;

using Newtonsoft.Json;

namespace commercetools.TaxCategories
{
    /// <summary>
    /// API representation for creating a new tax category.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-taxCategories.html#taxcategorydraft"/>
    public class TaxCategoryDraft
    {
        #region Properties

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "rates")]
        public List<TaxRateDraft> Rates { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="rates">Rates</param>
        public TaxCategoryDraft(string name, List<TaxRateDraft> rates)
        {
            this.Name = name;
            this.Rates = rates;
        }

        #endregion
    }
}
