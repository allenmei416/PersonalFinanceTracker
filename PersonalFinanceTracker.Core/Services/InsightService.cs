using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using PersonalFinanceTracker.Core.Dto;
using PersonalFinanceTracker.Core.Interfaces;
using System.Text;
using System.Text.Json;

namespace PersonalFinanceTracker.Core.Services
{
    public class InsightService : IInsightService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly string _apiKey;
        private const string CacheKey = "ai_insight";

        public InsightService(IHttpClientFactory httpClientFactory, IMemoryCache cache, IConfiguration config)
        {
            _httpClient = httpClientFactory.CreateClient("Anthropic");
            _cache = cache;
            _apiKey = config["Anthropic:ApiKey"] ?? throw new InvalidOperationException("Anthropic API key not configured");
        }

        public async Task<string> GetInsightAsync(IEnumerable<TransactionDto> transactions)
        {
            //return "💡 Spend less on Food: $120 this month Spend less on Food: $120 this month\n📉 Your spending is 18% higher than last month\n💰 You could save ~$200/month by reducing subscriptions";
            if (_cache.TryGetValue(CacheKey, out string? cached) && cached != null)
                return cached;

            var summary = transactions
                .GroupBy(t => t.CategoryName ?? "Uncategorized")
                .Select(g => $"{g.Key}: ${g.Sum(t => t.Amount):F2} across {g.Count()} transactions")
                .ToList();

            var totalSpend = transactions.Sum(t => t.Amount);
            var monthlyBreakdown = transactions
                .GroupBy(t => t.Date.ToString("MMM yyyy"))
                .Select(g => $"{g.Key}: ${g.Sum(t => t.Amount):F2}")
                .ToList();

            var prompt = $"""
                You are a personal finance assistant. Here is a summary of my recent transactions:

                Total Spend: ${totalSpend:F2}

                By Category:
                {string.Join("\n", summary)}

                By Month:
                {string.Join("\n", monthlyBreakdown)}

                Give me exactly 4 short, specific, actionable bullet points of financial insight based on this data. 
                Use dollar amounts where relevant. Be direct and concise. No intro or outro text, just the 4 bullets.
                Format each bullet starting with a relevant emoji.
                """;

            var requestBody = new
            {
                model = "claude-opus-4-5",
                max_tokens = 300,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "v1/messages");
            request.Headers.Add("x-api-key", _apiKey);
            request.Headers.Add("anthropic-version", "2023-06-01");
            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var insight = doc.RootElement
                .GetProperty("content")[0]
                .GetProperty("text")
                .GetString() ?? "No insight available.";

            _cache.Set(CacheKey, insight, TimeSpan.FromHours(24));

            return insight;
        }
    }
}