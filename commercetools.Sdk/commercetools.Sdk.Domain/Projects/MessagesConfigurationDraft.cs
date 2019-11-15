using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Projects
{
    public class MessagesConfigurationDraft
    {
        public bool Enabled { get; set; }
        [Required] 
        public int DeleteDaysAfterCreation { get; set; }
    }
}