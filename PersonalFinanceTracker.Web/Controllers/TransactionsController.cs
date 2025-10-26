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
    }
}
