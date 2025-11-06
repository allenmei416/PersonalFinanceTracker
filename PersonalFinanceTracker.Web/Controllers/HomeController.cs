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
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryDto>>("api/categories");
            var transactions = await _httpClient.GetFromJsonAsync<List<TransactionDto>>("api/transactions");
            return View(transactions, categories);
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

        [HttpPost]
        public async Task<IActionResult> AddTransaction(TransactionDto model)
        {
            if (!ModelState.IsValid)
            {
                // Reload Index with current transactions if validation fails
                var transactions = await _httpClient.GetFromJsonAsync<List<TransactionDto>>("api/transactions");
                return View("Index", transactions);
            }

            var dto = new CreateTransactionDto(model.CategoryId, model.Amount, model.Date, model.Note);

            var response = await _httpClient.PostAsJsonAsync("api/transactions", dto);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ShowModal = true;
                ViewBag.ErrorMessage = response.StatusCode;
            }

            var currentTransactions = await _httpClient.GetFromJsonAsync<List<TransactionDto>>("api/transactions");
            return View("Index", currentTransactions);
        }
    }
}
