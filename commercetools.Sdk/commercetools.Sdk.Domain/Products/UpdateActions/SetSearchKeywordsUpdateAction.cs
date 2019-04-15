using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class SetSearchKeywordsUpdateAction : UpdateAction<Product>
    {
        public string Action => "setSearchKeywords";
        [Required]
        public Dictionary<string, List<SearchKeywords>> SearchKeywords { get; set; }
        public bool Staged { get; set; }
    }
}