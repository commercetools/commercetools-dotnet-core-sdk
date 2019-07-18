using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.DiscountCodes.UpdateActions
{
    public class ChangeGroupsUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "changeGroups";
        [Required]
        public List<string> Groups { get; set; }
    }
}
