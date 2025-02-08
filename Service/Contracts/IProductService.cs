using Service.DTOs;

namespace Service.Contracts;

public interface IProductService
{
    Task<List<ProductGetDto>> GetProducts(int pageNumber, int pageSize);
    Task<List<ProductWithUserDto>> GetProductsWithUser(int pageNumber, int pageSize);
    Task<ProductGetDto> GetProduct(int id);
    Task<ProductWithUserDto> GetProductWithUser(int id);
    Task<int> InsertProduct(ProductAddEditDto productDto);
    Task UpdateProduct(ProductAddEditDto productDto);
    Task DeleteProduct(int id);
}
