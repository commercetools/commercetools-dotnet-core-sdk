using System.Net.Http;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace commercetools.Sdk.HttpApi
{
    public interface IApiExceptionFactory
    {
        ApiException CreateApiException(HttpRequestMessage request, HttpResponseMessage response);
    }
}