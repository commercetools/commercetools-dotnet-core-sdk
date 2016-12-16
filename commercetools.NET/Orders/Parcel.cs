using System;

using Newtonsoft.Json;

namespace commercetools.Orders
{
    /// <summary>
    /// A parcel stores the information about the appearance and the movement of a parcel.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#parcel"/>
    public class Parcel
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime? CreatedAt { get; private set; }

        [JsonProperty(PropertyName = "measurements")]
        public ParcelMeasurements Measurements { get; private set; }

        [JsonProperty(PropertyName = "trackingData")]
        public TrackingData TrackingData { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Parcel(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.Id = data.id;
            this.CreatedAt = data.createdAt;
            this.Measurements = new ParcelMeasurements(data.measurements);
            this.TrackingData = new TrackingData(data.trackingData);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Parcel parcel = obj as Parcel;

            if (parcel == null)
            {
                return false;
            }

            return (parcel.Id.Equals(this.Id));
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion
    }
}
