namespace SalePoint.Primitives.Interfaces
{
    public interface ISaleRepository
    {
        Task<List<Sale>> GetSalesByUserId(FilterSaleProducts filterSaleProducts);

        Task<int> ReturnProduct(ProductReturns productReturns);
    }
}