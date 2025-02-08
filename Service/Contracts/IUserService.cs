using Service.DTOs;

namespace Service.Contracts;
public interface IUserService
{
    Task<List<UserGetDto>> GetUsers(int pageNumber, int pageSize);
    Task<List<UserWithProductsDto>> GetUsersWithProducts(int pageNumber, int pageSize);
    Task<UserGetDto> GetUser(int id);
    Task<UserWithProductsDto> GetUserWithProducts(int id);
    Task<int> InsertUser(UserAddEditDto user);
    Task UpdateUser(UserAddEditDto user);
    Task AssignProductToUser(int userId, int productId);
    Task DeleteUser(int id);
}
