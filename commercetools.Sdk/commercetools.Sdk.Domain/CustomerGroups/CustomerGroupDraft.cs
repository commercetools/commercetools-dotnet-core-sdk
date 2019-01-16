using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.CustomerGroups
{
    public class CustomerGroupDraft : IDraft<CustomerGroup>
    {
        public string Key { get; set; }
        [Required]
        public string GroupName { get; set; }
        public CustomFieldsDraft Custom { get; set; }
    }
}