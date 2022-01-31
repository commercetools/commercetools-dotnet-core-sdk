using System;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.Projects
{
    public class SearchIndexingConfigurationValues
    {
        public SearchIndexingConfigurationStatus? Status { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public ClientLogging LastModifiedBy { get; set; }
    }
}