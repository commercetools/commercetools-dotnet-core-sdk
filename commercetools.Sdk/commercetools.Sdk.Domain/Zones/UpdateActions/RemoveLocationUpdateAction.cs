using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Zones.UpdateActions
{
    public class RemoveLocationUpdateAction : UpdateAction<Zone>
    {
        public string Action => "removeLocation";
        [Required]
        public Location Location { get; set; }
    }
}