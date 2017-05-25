using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.States
{
    /// <summary>
    /// A StateDraft is the representation to be given with a Create State request.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-states.html#statedraft"/>
    public class StateDraft
    {
        #region Properties

        /// <summary>
        /// Key
        /// </summary>
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        /// <summary>
        /// StateType
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StateType Type { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public LocalizedString Description { get; set; }

        /// <summary>
        /// Initial
        /// </summary>
        [JsonProperty(PropertyName = "initial")]
        public bool? Initial { get; set; }

        /// <summary>
        /// List of References to States
        /// </summary>
        [JsonProperty(PropertyName = "transitions")]
        public List<Reference> Transitions { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="type">StateType</param>
        public StateDraft(string key, StateType type)
        {
            this.Key = key;
            this.Type = type;
        }

        #endregion
    }
}
