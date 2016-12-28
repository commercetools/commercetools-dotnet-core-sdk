using System;
using System.Collections.Generic;

using commercetools.Common;
using commercetools.Messages;

using Newtonsoft.Json;

namespace commercetools.Project
{
    /// <summary>
    /// Project
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-project.html#project-1"/>
    public class Project
    {
        #region Properties

        [JsonProperty(PropertyName = "key")]
        public string Key { get; private set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "countries")]
        public List<string> Countries { get; private set; }

        [JsonProperty(PropertyName = "currencies")]
        public List<string> Currencies { get; private set; }

        [JsonProperty(PropertyName = "languages")]
        public List<string> Languages { get; private set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime? CreatedAt { get; private set; }

        [JsonProperty(PropertyName = "trialUntil")]
        public DateTime? TrialUntil { get; private set; }

        [JsonProperty(PropertyName = "messages")]
        public MessagesConfiguration Messages { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Project(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Key = data.key;
            this.Name = data.name;
            this.Countries = Helper.GetStringListFromJsonArray(data.countries);
            this.Currencies = Helper.GetStringListFromJsonArray(data.currencies);
            this.Languages = Helper.GetStringListFromJsonArray(data.languages);
            this.CreatedAt = data.createdAt;
            this.TrialUntil = data.trialUntil;
            this.Messages = new MessagesConfiguration(data.messages);
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
            Project project = obj as Project;

            if (project == null)
            {
                return false;
            }

            return project.Key.Equals(this.Key);
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Key.GetHashCode();
        }    

        #endregion
    }
}
