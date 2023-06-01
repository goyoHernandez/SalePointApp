using SalePoint.Primitives;
using SalePoint.Primitives.Interfaces;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace SalePoint.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        public ProductRepository(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> CreateProduct(Product product)
        {
            int productId = 0;
            try
            {
                HttpClient client = _httpClient.CreateClient("SalePoinApi");

                string? jsonString = JsonSerializer.Serialize(product);
                StringContent? requestContent = new(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage? response = await client.PostAsync("Product/Create", requestContent);

                if (response != null && response.IsSuccessStatusCode)
                {
                    string? content = await response.Content.ReadAsStringAsync();
                    productId = JsonSerializer.Deserialize<int>(content, options);
                }

                return productId;
            }
            catch
            {
                return productId;
            }
        }

        public async Task<IEnumerable<ProductModel>?> GetAllProducts()
        {
            IEnumerable<ProductModel>? products = null;
            try
            {
                var client = _httpClient.CreateClient("SalePoinApi");
                var response = await client.GetAsync("Product/All");

                if (response.IsSuccessStatusCode)
                {
                    string? content = await response.Content.ReadAsStringAsync();
                    products = JsonSerializer.Deserialize<List<ProductModel>>(content, options);
                }

                return products;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Product>?> GetProductsExpiringSoon()
        {
            IEnumerable<Product>? products = null;
            try
            {
                var client = _httpClient.CreateClient("SalePoinApi");
                var response = await client.GetAsync("Product/ExpiringSoon");

                if (response.IsSuccessStatusCode)
                {
                    string? content = await response.Content.ReadAsStringAsync();
                    products = JsonSerializer.Deserialize<List<Product>>(content, options);
                }

                return products;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Product>?> GetProductsNearCompletition()
        {
            IEnumerable<Product>? products = null;
            try
            {
                var client = _httpClient.CreateClient("SalePoinApi");
                var response = await client.GetAsync("Product/NearCompletition");

                if (response.IsSuccessStatusCode)
                {
                    string? content = await response.Content.ReadAsStringAsync();
                    products = JsonSerializer.Deserialize<List<Product>>(content, options);
                }

                return products;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Product?> GetProductById(int productId)
        {
            Product? product = new();
            try
            {
                var client = _httpClient.CreateClient("SalePoinApi");
                var response = await client.GetAsync($"Product/Get/productId/{productId}");

                if (response.IsSuccessStatusCode)
                {
                    string? content = await response.Content.ReadAsStringAsync();
                    product = JsonSerializer.Deserialize<Product?>(content, options);
                }

                return product;
            }
            catch
            {
                return product;
            }
        }

        public async Task<IEnumerable<Product>?> GetProductByBarCode(string barCode)
        {
            try
            {
                IEnumerable<Product>? products = null;
                var client = _httpClient.CreateClient("SalePoinApi");
                var response = await client.GetAsync($"Product/GetBy/barCode/{barCode}");

                if (response.IsSuccessStatusCode)
                {
                    string? content = await response.Content.ReadAsStringAsync();
                    products = JsonSerializer.Deserialize<List<Product>>(content, options);
                }

                return products;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProductModel>?> GetProductByNameOrDescription(string keyWord)
        {
            try
            {
                IEnumerable<ProductModel>? products = null;
                var client = _httpClient.CreateClient("SalePoinApi");
                var response = await client.GetAsync($"Product/GetBy/NameOrDescription/{keyWord}");

                if (response.IsSuccessStatusCode)
                {
                    string? content = await response.Content.ReadAsStringAsync();
                    products = JsonSerializer.Deserialize<List<ProductModel>>(content, options);
                }

                return products;
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> UpdateProduct(Product product)
        {
            int productId = 0;
            try
            {
                HttpClient client = _httpClient.CreateClient("SalePoinApi");

                string? jsonString = JsonSerializer.Serialize(product);
                StringContent? requestContent = new(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage? response = await client.PutAsync("Product/Update", requestContent);

                if (response != null && response.IsSuccessStatusCode)
                {
                    string? content = await response.Content.ReadAsStringAsync();
                    productId = JsonSerializer.Deserialize<int>(content, options);
                }

                return productId;
            }
            catch
            {
                return productId;
            }
        }

        public async Task<int> UpdateStockProduct(int stock, int productId)
        {
            int id = 0;
            try
            {
                HttpClient client = _httpClient.CreateClient("SalePoinApi");

                string? jsonString = JsonSerializer.Serialize(stock);
                StringContent? requestContent = new(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage? response = await client.PutAsync($"Product/Update/{productId}/stock", requestContent);

                if (response != null && response.IsSuccessStatusCode)
                {
                    string? content = await response.Content.ReadAsStringAsync();
                    id = JsonSerializer.Deserialize<int>(content, options);
                }

                return id;
            }
            catch
            {
                return id;
            }
        }

        public async Task<int> DeleteProduct(int id, int userId)
        {
            try
            {
                int productId = 0;
                HttpClient client = _httpClient.CreateClient("SalePoinApi");

                HttpResponseMessage? response = await client.DeleteAsync($"Product/Delete/id/{id}/userId/{userId}");

                if (response != null && response.IsSuccessStatusCode)
                {
                    string? content = await response.Content.ReadAsStringAsync();
                    productId = JsonSerializer.Deserialize<int>(content, options);
                }

                return productId;
            }
            catch
            {
                throw;
            }
        }
    }
}