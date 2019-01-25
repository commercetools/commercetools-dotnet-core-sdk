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
        private readonly ITokenFlowMapper tokenFlowMapper;

        public HomeController(IClient client, ITokenFlowMapper tokenFlowMapper)
        {
            this.client = client;
            this.tokenFlowMapper = tokenFlowMapper;
        }

        public IActionResult Index()
        {
            PagedQueryResult<Category> category = this.client.ExecuteAsync(new QueryCommand<Category>()).Result;
            int count = category.Results.Count;
            return View(new { CategoryCount = count, TokenFlowRegister = tokenFlowMapper.TokenFlowRegister.TokenFlow });
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            tokenFlowMapper.TokenFlowRegister.TokenFlow = TokenFlow.Password;
            PagedQueryResult<Category> category = this.client.ExecuteAsync(new QueryCommand<Category>()).Result;
            int count = category.Results.Count;
            return View(new { CategoryCount = count, TokenFlowRegister = tokenFlowMapper.TokenFlowRegister.TokenFlow });
        }
    }
}