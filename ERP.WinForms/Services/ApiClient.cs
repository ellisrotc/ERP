using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ERP.WinForms.Services;

public sealed class ApiClient
{
    private static readonly ApiClient _instance = new();
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

    private ApiClient()
    {
        _http = new HttpClient { BaseAddress = new Uri("http://localhost:5260") };
        _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public static ApiClient Instance => _instance;

    public void SetToken(string jwt)
    {
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", jwt);
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        var resp = await _http.GetAsync(url);
        resp.EnsureSuccessStatusCode();
        var json = await resp.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, JsonOpts);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
    {
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var resp = await _http.PostAsync(url, content);
        resp.EnsureSuccessStatusCode();
        var respJson = await resp.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(respJson, JsonOpts);
    }

    public async Task<byte[]> GetBytesAsync(string url)
    {
        var resp = await _http.GetAsync(url);
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadAsByteArrayAsync();
    }

    public async Task DeleteAsync(string url)
    {
        var resp = await _http.DeleteAsync(url);
        resp.EnsureSuccessStatusCode();
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data)
    {
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var resp = await _http.PutAsync(url, content);
        resp.EnsureSuccessStatusCode();
        var respJson = await resp.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(respJson, JsonOpts);
    }
}
