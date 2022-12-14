using System.Net.Http.Headers;
using SimpleWebApp.Core.Models;

Asker.Fetch<Product>();
Asker.Fetch<Category>();

internal static class Asker
{
    public static void Fetch<T>()
    {
        string url = String.Empty;
        if (typeof(T) == typeof(Product))
            url = "https://localhost:5001/Api/Products";
        else if (typeof(T) == typeof(Category))
            url = "https://localhost:5001/Api/Categories";
        var response = GetResponse(url);
        if (response.IsSuccessStatusCode)
        {
            var dataObjects = response.Content.ReadAsAsync<IEnumerable<T>>().Result;
            foreach (var d in dataObjects)
            {
                Console.WriteLine(d.ToString());
            }
        }
    }

    private static HttpResponseMessage GetResponse(string url)
    {
        using var client= new HttpClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(url);
        HttpResponseMessage response = client.GetAsync("").Result;
        return response;
    }
}

