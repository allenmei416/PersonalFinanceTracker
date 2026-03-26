using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.Core.Interfaces;

namespace PersonalFinanceTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsightController : ControllerBase
    {
        private readonly IInsightService _insightService;
        private readonly ITransactionService _transactionService;

        public InsightController(IInsightService insightService, ITransactionService transactionService)
        {
            _insightService = insightService;
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var transactions = await _transactionService.GetAllAsync();
            var insight = await _insightService.GetInsightAsync(transactions);
            return Ok(new { insight });
        }
    }
}