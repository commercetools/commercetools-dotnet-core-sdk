using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Zones.UpdateActions
{
    public class AddLocationUpdateAction : UpdateAction<Zone>
    {
        public string Action => "addLocation";
        [Required]
        public Location Location { get; set; }
    }
}