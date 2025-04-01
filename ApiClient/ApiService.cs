using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using laba2.Models;

public class ApiService
{
    private readonly HttpClient _http;

    public ApiService()
    {
        _http = new HttpClient { BaseAddress = new Uri("http://localhost:5200/api/") };
    }

    public async Task<List<Rabbit>> GetRabbits()
    {
        try
        {
            var rabbits = await _http.GetFromJsonAsync<List<Rabbit>>("rabbits"); 
            return rabbits ?? new List<Rabbit>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при получении кроликов: {ex.Message}");
            return new List<Rabbit>();
        }
    }

    public async Task AddRabbit(Rabbit rabbit)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("rabbits", rabbit); // 👈 исправил `Rabbit` на `rabbits`
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при добавлении кролика: {ex.Message}");
        }
    }

    public async Task UpdateRabbit(Rabbit rabbit)
    {
        try
        {
            var response = await _http.PutAsJsonAsync($"rabbits/{rabbit.Id}", rabbit); 
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при обновлении кролика: {ex.Message}");
        }
    }

    public async Task DeleteRabbit(int id) 
    {
        try
        {
            var response = await _http.DeleteAsync($"rabbits/{id}"); 
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при удалении кролика: {ex.Message}");
        }
    }
    public async Task<List<Rabbit>> SearchRabbits(string query)
    {
        try
        {
            var rabbits = await _http.GetFromJsonAsync<List<Rabbit>>($"rabbits/search?query={query}");
            return rabbits ?? new List<Rabbit>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при поиске кроликов: {ex.Message}");
            return new List<Rabbit>();
        }
    }

}
