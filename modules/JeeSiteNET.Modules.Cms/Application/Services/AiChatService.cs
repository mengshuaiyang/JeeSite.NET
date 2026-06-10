using System.Net.Http.Json;
using System.Text.Json;
using JeeSiteNET.Modules.Cms.Application.DTOs;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JeeSiteNET.Modules.Cms.Application.Services;

public class AiChatService
{
    private readonly IArticleRepository _articleRepository;
    private readonly IArticleDataRepository _articleDataRepository;
    private readonly HttpClient _http;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AiChatService> _logger;

    public AiChatService(
        IArticleRepository articleRepository,
        IArticleDataRepository articleDataRepository,
        HttpClient http,
        IConfiguration configuration,
        ILogger<AiChatService> logger)
    {
        _articleRepository = articleRepository;
        _articleDataRepository = articleDataRepository;
        _http = http;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<AiChatResponse> ChatAsync(AiChatRequest request)
    {
        var section = _configuration.GetSection("AI");
        var apiUrl = section["ApiUrl"] ?? "https://api.deepseek.com/v1/chat/completions";
        var apiKey = section["ApiKey"] ?? "";
        var model = section["Model"] ?? "deepseek-chat";
        var maxContextArticles = int.Parse(section["MaxContextArticles"] ?? "5");

        var context = await BuildContextAsync(request.Message, request.CategoryCode, maxContextArticles);

        var messages = new List<object>();
        messages.Add(new
        {
            role = "system",
            content = $"你是一个 CMS 内容助手，基于以下文章内容回答用户问题。如果问题与文章无关，请如实告知。\n\n参考文章：\n{context.Summary}"
        });

        if (request.History != null)
        {
            foreach (var msg in request.History.TakeLast(10))
                messages.Add(new { role = msg.Role, content = msg.Content });
        }

        messages.Add(new { role = "user", content = request.Message });

        var body = new
        {
            model,
            messages,
            temperature = 0.7,
            max_tokens = 2048,
        };

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, apiUrl);
        httpRequest.Headers.Add("Authorization", $"Bearer {apiKey}");
        httpRequest.Content = JsonContent.Create(body);

        var response = await _http.SendAsync(httpRequest);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        var reply = "";
        if (result.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
        {
            var choice = choices[0];
            if (choice.TryGetProperty("message", out var msg))
                reply = msg.GetProperty("content").GetString() ?? "";
            else if (choice.TryGetProperty("delta", out var delta))
                reply = delta.GetProperty("content").GetString() ?? "";
        }

        return new AiChatResponse
        {
            Reply = reply,
            SourceArticles = context.Titles,
        };
    }

    private async Task<(string Summary, List<string> Titles)> BuildContextAsync(string query, string? categoryCode, int maxArticles)
    {
        var pageRequest = new Core.PageRequest<Domain.Entities.Article>
        {
            PageSize = maxArticles,
            PageNo = 1,
            Entity = !string.IsNullOrEmpty(categoryCode) ? new Domain.Entities.Article { CategoryCode = categoryCode } : null,
        };

        var pageResult = await _articleRepository.FindPageAsync(pageRequest);

        var summaryParts = new List<string>();
        var titles = new List<string>();

        foreach (var article in pageResult.List.Take(maxArticles))
        {
            var data = await _articleDataRepository.GetAsync(article.ArticleCode);
            var content = data?.Content ?? "";
            var snippet = content.Length > 500 ? content[..500] + "..." : content;

            summaryParts.Add($"【{article.Title}】\n{snippet}\n");
            titles.Add(article.Title);
        }

        return (string.Join("\n---\n", summaryParts), titles);
    }
}
