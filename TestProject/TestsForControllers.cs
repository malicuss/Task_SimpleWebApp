using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IO.Swagger.Api;
using IO.Swagger.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using SimpleWebApp.Models;
using Xunit;

namespace TestProject;

public class BasicTests: IClassFixture<WebApplicationFactory<Program>>
{
    internal class ApiTestStubs
    {
        public Product? ProductA { get; init; }
        public Product? ProductB { get; init; }

        public Category CategoryA { get; init; }
        public Category CategoryB { get; init; }

        public List<Product?> Products { get; init; }
        public List<Category?> Categories { get; init; }

        public ApiTestStubs()
        {
            ProductA = new Product
            {
                ProductId = 0,
                ProductName = "ProductA",
                SupplierId = 1,
                CategoryId = 1,
                QuantityPerUnit = "some string",
                UnitPrice = 10,
                UnitsInStock = 10,
                UnitsOnOrder = 10,
                ReorderLevel = 1,
                Discontinued = false
            };
            ProductB = new Product
            {
                ProductId = 100,
                ProductName = "ProductB",
                SupplierId = 2,
                CategoryId = 2,
                QuantityPerUnit = "some string 2",
                UnitPrice = 20,
                UnitsInStock = 20,
                UnitsOnOrder = 20,
                ReorderLevel = 2,
                Discontinued = true
            };
        }
    }
    
    private readonly WebApplicationFactory<Program> _factory;
    private ApiApi _apiApi;
    private ApiTestStubs _testStubs;

    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _testStubs = new ApiTestStubs();
        _apiApi = new ApiApi("https://localhost:7263");
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

    [Fact]
    public async Task API_GetProducts()
    {
        var prods = _apiApi.ApiGetProductsGet();
        Assert.True(prods.Count>0);
    }

    [Fact]
    public async Task API_CreateProduct()
    {
        var res = await _apiApi.ApiCreateProductPostAsync(
            _testStubs.ProductA.ProductId,
            _testStubs.ProductA.ProductName,
            _testStubs.ProductA.SupplierId,
            _testStubs.ProductA.CategoryId,
            _testStubs.ProductA.QuantityPerUnit,
            (double)_testStubs.ProductA.UnitPrice,
            _testStubs.ProductA.UnitsInStock,
            _testStubs.ProductA.UnitsOnOrder,
            _testStubs.ProductA.ReorderLevel,
            _testStubs.ProductA.Discontinued);
        Assert.True(res);
    }

    [Fact]
    public async Task API_UpdateProduct()
    {
        var tmp = await _apiApi.ApiCreateProductPostAsync(
            _testStubs.ProductA.ProductId,
            _testStubs.ProductA.ProductName,
            _testStubs.ProductA.SupplierId,
            _testStubs.ProductA.CategoryId,
            _testStubs.ProductA.QuantityPerUnit,
            (double)_testStubs.ProductA.UnitPrice,
            _testStubs.ProductA.UnitsInStock,
            _testStubs.ProductA.UnitsOnOrder,
            _testStubs.ProductA.ReorderLevel,
            _testStubs.ProductA.Discontinued);
        
        var id = _apiApi.ApiGetProductsGet().Last().ProductId;
        
        var res = _apiApi.ApiUpdateProductPost(
            id,
            _testStubs.ProductB.ProductName,
            _testStubs.ProductB.SupplierId,
            _testStubs.ProductB.CategoryId,
            _testStubs.ProductB.QuantityPerUnit,
            (double)_testStubs.ProductB.UnitPrice,
            _testStubs.ProductB.UnitsInStock,
            _testStubs.ProductB.UnitsOnOrder,
            _testStubs.ProductB.ReorderLevel,
            _testStubs.ProductB.Discontinued);
        Assert.True(res);
    }
    [Fact]
    public async Task API_DeleteProduct()
    {
        //TODO: not possible to implement without product search
        Assert.True(true);
    }
}