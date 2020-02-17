using System.Collections.Generic;
using commercetools.Sdk.Domain.Errors;

namespace commercetools.Sdk.Domain.OrderEdits
{
    [TypeMarker("PreviewFailure")]
    public class OrderEditPreviewFailure : OrderEditResult
    {
        public List<ErrorResponse> Errors { get; set; }
    }
}