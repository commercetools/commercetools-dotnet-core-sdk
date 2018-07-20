using commercetools.Common;
using commercetools.CustomFields;
using commercetools.GeoLocation;
using commercetools.Reviews;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace commercetools.Channels
{
    /// <summary>
    /// Channels represent a source or destination of different entities. They can be used to model warehouses or stores.
    /// </summary>
    /// <see href="http://docs.commercetools.com/http-api-projects-channels.html#channel"/>
    public class Channel
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "version")]
        public int Version { get; private set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime CreatedAt { get; private set; }

        [JsonProperty(PropertyName = "lastModifiedAt")]
        public DateTime LastModifiedAt { get; private set; }

        [JsonProperty(PropertyName = "key")]
        public string Key { get; private set; }

        [JsonProperty(PropertyName = "roles")]
        [JsonConverter(typeof(StringEnumConverter))]
        public List<ChannelRoleEnum> Roles { get; private set; }

        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; private set; }

        [JsonProperty(PropertyName = "description")]
        public LocalizedString Description { get; private set; }

        [JsonProperty(PropertyName = "address")]
        public Address Address { get; private set; }

        [JsonProperty(PropertyName = "reviewRatingStatistics")]
        public ReviewRatingStatistics ReviewRatingStatistics { get; private set; }

        [JsonProperty(PropertyName = "custom")]
        public CustomFields.CustomFields Custom { get; private set; }

        [JsonProperty(PropertyName = "geoLocation")]
        public IGeoLocationObject GeoLocation { get; private set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Channel(dynamic data)
        {
            if (data == null)
            {
                return;
            }
                        
            this.Id = data.id;
            this.Version = data.version;
            this.CreatedAt = data.createdAt;
            this.LastModifiedAt = data.lastModifiedAt;
            this.Key = data.key;
            this.Name = data.name != null ? new LocalizedString(data.name) : null;
            this.Description = data.description != null ? new LocalizedString(data.description) : null;
            this.Address = data.address != null ? new Address(data.address) : null;
            this.ReviewRatingStatistics = data.reviewRatingStatistics != null ? new ReviewRatingStatistics(data.reviewRatingStatistics) : null;
            this.Custom = new CustomFields.CustomFields(data.custom);
            if (data.geoLocation != null && data.geoLocation.type != null)
            {
                switch ((string)data.geoLocation.type)
                {
                    case "Point":
                        this.GeoLocation = new Point(data.geoLocation);
                        break;
                    default:
                        this.GeoLocation = new Geometry(data);
                        break;
                }
            }
            if (data.roles != null)
            {
                this.Roles = new List<ChannelRoleEnum>();
                foreach (dynamic role in data.roles)
                {
                    ChannelRoleEnum thisRole;

                    if (Enum.TryParse((string)role, out thisRole))
                    {
                        this.Roles.Add(thisRole);
                    }
                }
            }
            
        }
        #endregion
    }
}
