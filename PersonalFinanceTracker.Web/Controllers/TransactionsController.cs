using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.Core.Dto;

namespace PersonalFinanceTracker.Web.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly HttpClient _httpClient;

        public TransactionsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }
        public async Task< IActionResult> Index()
        {
            var transactions = await _httpClient.GetFromJsonAsync<List<TransactionDto>>("api/transactions");
            return View(transactions);
        }

        [HttpPost]
        public async Task<IActionResult> AddTransactionAsync(TransactionDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _httpClient.PostAsJsonAsync("api/Transactions", model);

            if (response.IsSuccessStatusCode)
            {
                // Optional: Fetch updated transactions and return to index view
                var transactions = await _httpClient.GetFromJsonAsync<List<TransactionDto>>("api/Transactions");
                return View(transactions);
            }

            ModelState.AddModelError(string.Empty, "Error saving transaction to API.");
            return View(model);
        }
    }
}
