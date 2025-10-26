using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.Core.Dto;
using PersonalFinanceTracker.Web.Models;
using System.Diagnostics;
using System.Net.Http;

namespace PersonalFinanceTracker.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("API");
        }

        public async Task<IActionResult> Index()
        {
            var transactions = await _httpClient.GetFromJsonAsync<List<TransactionDto>>("api/transactions");
            return View(transactions);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
