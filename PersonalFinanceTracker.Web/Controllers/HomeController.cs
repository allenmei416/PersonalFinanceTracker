using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.Core.Dto;
using PersonalFinanceTracker.Web.Models;
using System.Diagnostics;

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

        // --- INDEX PAGE ---
        public async Task<IActionResult> Index()
        {
            var transactions = await _httpClient.GetFromJsonAsync<List<TransactionDto>>("api/transactions");
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryDto>>("api/categories");

            var vm = new DashboardViewModel
            {
                Transactions = transactions ?? new List<TransactionDto>(),
                Categories = categories ?? new List<CategoryDto>()
            };

            return View(vm);
        }


        // --- PRIVACY PAGE ---
        public IActionResult Privacy()
        {
            return View();
        }

        // --- ERROR PAGE ---
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // --- ADD TRANSACTION ---
        [HttpPost]
        public async Task<IActionResult> AddTransaction(TransactionDto model)
        {
            // Always reload categories for the dropdown
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryDto>>("api/categories");

            if (!ModelState.IsValid)
            {
                var transactions = await _httpClient.GetFromJsonAsync<List<TransactionDto>>("api/transactions");
                var vm = new DashboardViewModel
                {
                    Transactions = transactions,
                    Categories = categories
                };
                return View("Index", vm);
            }

            var dto = new CreateTransactionDto(model.CategoryId, model.Amount, model.Date, model.Note);
            var response = await _httpClient.PostAsJsonAsync("api/transactions", dto);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ShowModal = true;
                ViewBag.ErrorMessage = $"Error: {response.StatusCode}";

                var transactions = await _httpClient.GetFromJsonAsync<List<TransactionDto>>("api/transactions");
                var vm = new DashboardViewModel
                {
                    Transactions = transactions,
                    Categories = categories
                };
                return View("Index", vm);
            }

            // Refresh the dashboard with the updated transactions
            var updatedTransactions = await _httpClient.GetFromJsonAsync<List<TransactionDto>>("api/transactions");

            var updatedVm = new DashboardViewModel
            {
                Transactions = updatedTransactions,
                Categories = categories
            };

            return View("Index", updatedVm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/transactions/{id}");

            // Always reload transactions and categories for refreshing the view
            var transactions = await _httpClient.GetFromJsonAsync<List<TransactionDto>>("api/transactions");
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryDto>>("api/categories");

            var vm = new DashboardViewModel
            {
                Transactions = transactions ?? new List<TransactionDto>(),
                Categories = categories ?? new List<CategoryDto>()
            };

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

            var vm = new DashboardViewModel
            {
                Transactions = transactions ?? new List<TransactionDto>(),
                Categories = categories ?? new List<CategoryDto>()
            };

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMessage = $"Failed to update transaction (Status: {response.StatusCode}).";
            }
            ModelState.Clear();

            return View("Index", vm);
        }

    }
}
