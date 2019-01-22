using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.HttpApi.Tokens;
using Microsoft.AspNetCore.Mvc;

namespace commercetools.Sdk.AnonymousSessionExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IClient client;
        private readonly ITokenFlowRegister tokenFlowRegister;

        public HomeController(IClient client, ITokenFlowRegister tokenFlowRegister)
        {
            this.client = client;
            this.tokenFlowRegister = tokenFlowRegister;
        }

        public IActionResult Index()
        {
            PagedQueryResult<Category> category = this.client.ExecuteAsync(new QueryCommand<Category>()).Result;
            int count = category.Results.Count;
            return View(new { CategoryCount = count, TokenFlowRegister = tokenFlowRegister.TokenFlow });
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            tokenFlowRegister.TokenFlow = TokenFlow.Password;
            PagedQueryResult<Category> category = this.client.ExecuteAsync(new QueryCommand<Category>()).Result;
            int count = category.Results.Count;
            return View(new { CategoryCount = count, TokenFlowRegister = tokenFlowRegister.TokenFlow });
        }
    }
}