namespace SalePoint.Primitives.Interfaces
{
    public interface IProductRepository
    {
        Task<int> CreateProduct(Product product, string token);

        Task<IEnumerable<ProductModel>?> GetAllProducts(string token);

        Task<IEnumerable<Product>?> GetProductsExpiringSoon(string token);

        Task<IEnumerable<Product>?> GetProductsNearCompletition(string token);

        Task<Product?> GetProductById(int productId, string token);

        Task<IEnumerable<Product>?> GetProductByBarCode(string barCode, string token);

        Task<IEnumerable<ProductModel>?> GetProductByNameOrDescription(string keyWord, string token);

        Task<int> UpdateProduct(Product product, string token);

        Task<int> UpdateStockProduct(int idProduct, int stock, string token);

        Task<int> DeleteProduct(int id, int userId, string token);
    }
}