using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace commercetools.Sdk.DependencyInjection.SimpleInjector
{
    public class ApplicationServicesWrapper : IApplicationBuilder
    {
        public IServiceProvider ApplicationServices { get; set; }

        IFeatureCollection IApplicationBuilder.ServerFeatures => throw new Exception();
        IDictionary<string, object> IApplicationBuilder.Properties => throw new Exception();
        RequestDelegate IApplicationBuilder.Build() => throw new Exception();
        IApplicationBuilder IApplicationBuilder.New() => throw new Exception();
        IApplicationBuilder IApplicationBuilder.Use(Func<RequestDelegate, RequestDelegate> middleware)
            => throw new Exception();
    }
}
