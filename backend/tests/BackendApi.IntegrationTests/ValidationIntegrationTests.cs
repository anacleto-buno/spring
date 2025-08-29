using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using BackendApi.DTOs.Product;

namespace BackendApi.IntegrationTests
{
	public class ValidationIntegrationTests : IClassFixture<CustomWebApplicationFactory>
	{
		private readonly HttpClient _client;
		private readonly CustomWebApplicationFactory _factory;

		public ValidationIntegrationTests(CustomWebApplicationFactory factory)
		{
			_factory = factory;
			_client = factory.CreateClient();
		}

		[Fact]
		public async Task GetProducts_SimpleTest()
		{
			var response = await _client.GetAsync("/api/products");
			var body = await response.Content.ReadAsStringAsync();
			Console.WriteLine($"Status: {response.StatusCode}\nBody: {body}");
			Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest, $"Unexpected status: {response.StatusCode}");
		}

		[Fact]
		public async Task CreateProduct_InvalidModel_ReturnsValidationError()
		{
			var invalidProduct = new ProductCreateDto { Name = "", Price = 0 };
			var response = await _client.PostAsJsonAsync("/api/products", invalidProduct);
			var error = await response.Content.ReadAsStringAsync();
			Console.WriteLine($"Status: {response.StatusCode}\nBody: {error}");
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
			Assert.Contains("Product name is required.", error);
			Assert.Contains("Price must be greater than zero.", error);
		}

		[Fact]
		public async Task UpdateProduct_InvalidModel_ReturnsValidationError()
		{
			// First, create a valid product
			var validProduct = new ProductCreateDto { Name = "Test", Price = 10, SKU = "TESTSKU" };
			var createResponse = await _client.PostAsJsonAsync("/api/products", validProduct);
			createResponse.EnsureSuccessStatusCode();
			var created = await createResponse.Content.ReadFromJsonAsync<ProductResponseDto>();
			Assert.NotNull(created);

			// Now, try to update with invalid data
			var invalidProduct = new ProductUpdateDto { Name = "", Price = 0, SKU = "TESTSKU" };
			var response = await _client.PutAsJsonAsync($"/api/products/{created!.Id}", invalidProduct);
			var error = await response.Content.ReadAsStringAsync();
			Console.WriteLine($"Status: {response.StatusCode}\nBody: {error}");
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
			Assert.Contains("Product name is required.", error);
			Assert.Contains("Price must be greater than zero.", error);
		}

		[Fact]
		public async Task FilterProducts_InvalidPageSize_ClampedToMax()
		{
			var filter = new ProductFilterDto { Page = 1, PageSize = 101 };
			var response = await _client.PostAsJsonAsync("/api/products/filter", filter);
			var result = await response.Content.ReadAsStringAsync();
			Console.WriteLine($"Status: {response.StatusCode}\nBody: {result}");
			response.EnsureSuccessStatusCode();
			Assert.Contains("\"pageSize\":100", result);
		}
	}
}
