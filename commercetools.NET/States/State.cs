using System;
using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.States
{
    /// <summary>
    /// A State represents a state of a particular entity (defines a finite state machine).
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-states.html#state"/>
    public class State
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "version")]
        public int? Version { get; private set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime? CreatedAt { get; private set; }

        [JsonProperty(PropertyName = "lastModifiedAt")]
        public DateTime? LastModifiedAt { get; private set; }

        [JsonProperty(PropertyName = "key")]
        public string Key { get; private set; }

        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StateType? Type { get; private set; }

        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; private set; }

        [JsonProperty(PropertyName = "description")]
        public LocalizedString Description { get; private set; }

        [JsonProperty(PropertyName = "initial")]
        public bool? Initial { get; private set; }

        [JsonProperty(PropertyName = "builtIn")]
        public bool? BuiltIn { get; private set; }

        [JsonProperty(PropertyName = "roles")]
        public List<StateRole> Roles { get; private set; }

        [JsonProperty(PropertyName = "transitions")]
        public List<Reference> Transitions { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public State(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            StateType type;

            string typeStr = (data.taxMode != null ? data.taxMode.ToString() : string.Empty);

            this.Id = data.id;
            this.Version = data.version;
            this.CreatedAt = data.createdAt;
            this.LastModifiedAt = data.lastModifiedAt;
            this.Key = data.key;
            this.Type = Enum.TryParse(typeStr, out type) ? (StateType?)type : null;
            this.Name = data.name != null ? new LocalizedString(data.name) : null;
            this.Description = data.description != null ? new LocalizedString(data.description) : null;
            this.Initial = data.initial;
            this.BuiltIn = data.builtIn;
            this.Transitions = Helper.GetListFromJsonArray<Reference>(data.transitions);

            if (data.roles != null)
            {
                this.Roles = new List<StateRole>();

                foreach (dynamic role in data.roles)
                {
                    StateRole thisRole;

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
