using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.Core.Dto;
using PersonalFinanceTracker.Web.Models;
using System.Diagnostics;
using System.Text.Json;

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
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryDto>>("api/categories");
            var insight = await FetchInsightAsync();

            var vm = BuildViewModel(
                transactions ?? new List<TransactionDto>(),
                categories ?? new List<CategoryDto>(),
                insight
            );

            return View(vm);
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
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryDto>>("api/categories");

            if (!ModelState.IsValid)
            {
                var transactions = await _httpClient.GetFromJsonAsync<List<TransactionDto>>("api/transactions");
                var insight = await FetchInsightAsync();
                var vm = BuildViewModel(
                    transactions ?? new List<TransactionDto>(),
                    categories ?? new List<CategoryDto>(),
                    insight
                );
                return View("Index", vm);
            }

            var dto = new CreateTransactionDto(model.CategoryId, model.Amount, model.Date, model.Note);
            var response = await _httpClient.PostAsJsonAsync("api/transactions", dto);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ShowModal = true;
                ViewBag.ErrorMessage = $"Error: {response.StatusCode}";

                var transactions = await _httpClient.GetFromJsonAsync<List<TransactionDto>>("api/transactions");
                var insight = await FetchInsightAsync();
                var vm = BuildViewModel(
                    transactions ?? new List<TransactionDto>(),
                    categories ?? new List<CategoryDto>(),
                    insight
                );
                return View("Index", vm);
            }

            var updatedTransactions = await _httpClient.GetFromJsonAsync<List<TransactionDto>>("api/transactions");
            var updatedInsight = await FetchInsightAsync();

            var updatedVm = BuildViewModel(
                updatedTransactions ?? new List<TransactionDto>(),
                categories ?? new List<CategoryDto>(),
                updatedInsight
            );

            return View("Index", updatedVm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/transactions/{id}");

            var transactions = await _httpClient.GetFromJsonAsync<List<TransactionDto>>("api/transactions");
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryDto>>("api/categories");
            var insight = await FetchInsightAsync();

            var vm = BuildViewModel(
                transactions ?? new List<TransactionDto>(),
                categories ?? new List<CategoryDto>(),
                insight
            );

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ShowModal = false;
                ViewBag.ErrorMessage = $"Failed to delete transaction (Status: {response.StatusCode}).";
            }

            return View("Index", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditTransaction(TransactionDto model)
        {
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryDto>>("api/categories");

            var dto = new UpdateTransactionDto
            {
                CategoryId = model.CategoryId,
                Amount = model.Amount,
                Date = model.Date,
                Note = model.Note
            };

            var response = await _httpClient.PutAsJsonAsync($"api/transactions/{model.TransactionId}", dto);

            var transactions = await _httpClient.GetFromJsonAsync<List<TransactionDto>>("api/transactions");
            var insight = await FetchInsightAsync();

            var vm = BuildViewModel(
                transactions ?? new List<TransactionDto>(),
                categories ?? new List<CategoryDto>(),
                insight
            );

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMessage = $"Failed to update transaction (Status: {response.StatusCode}).";
            }

            ModelState.Clear();
            return View("Index", vm);
        }

        private async Task<string> FetchInsightAsync()
        {
            try
            {
                var insightResponse = await _httpClient.GetFromJsonAsync<JsonElement>("api/insight");
                return insightResponse.GetProperty("insight").GetString() ?? string.Empty;
            }
            catch
            {
                return "Unable to load insight at this time.";
            }
        }

        private DashboardViewModel BuildViewModel(
            List<TransactionDto> transactions,
            List<CategoryDto> categories,
            string insight = "")
        {
            var spendingByCategory = transactions
                .GroupBy(t => t.CategoryName ?? "Uncategorized")
                .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));

            var sixMonthsAgo = DateTime.Today.AddMonths(-5);
            var monthlySpend = transactions
                .Where(t => t.Date >= new DateTime(sixMonthsAgo.Year, sixMonthsAgo.Month, 1))
                .GroupBy(t => new { t.Date.Year, t.Date.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .ToDictionary(
                    g => new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy"),
                    g => g.Sum(t => t.Amount)
                );

            return new DashboardViewModel
            {
                Transactions = transactions,
                Categories = categories,
                SpendingByCategory = spendingByCategory,
                MonthlySpend = monthlySpend,
                AiInsight = insight
            };
        }
    }
}