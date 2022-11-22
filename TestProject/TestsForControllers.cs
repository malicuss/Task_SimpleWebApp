using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TestProject;

public class BasicTests: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Home")]
    [InlineData("/Categories/List")]
    [InlineData("/Categories/ImageForm")]
    [InlineData("/Categories/ImageForm?ImageId=1")]
    [InlineData("/Image/1")]
    [InlineData("/Products/List")]
    [InlineData("/Products/AddUpdateProduct?ProductId=1")]
    public async Task Get_EndpointsReturn(string url)
    {
        var client = _factory.CreateClient();
        var resp = await client.GetAsync(url);
        
        resp.EnsureSuccessStatusCode();
        Assert.Equal("text/html; charset=utf-8", 
            resp.Content.Headers.ContentType.ToString());
    }
}