using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using Microsoft.AspNetCore.Mvc;

namespace commercetools.Sdk.AnonymousSessionExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IClient client;

        public HomeController(IClient client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            PagedQueryResult<Category> category = this.client.ExecuteAsync(new QueryCommand<Category>()).Result;
            return this.Content(category.Results.Count.ToString());
        }
    }
}