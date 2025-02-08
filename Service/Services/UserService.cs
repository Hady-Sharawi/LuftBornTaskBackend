using AutoMapper;
using Entities.Domains;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Repository;
using Service.Contracts;
using Service.DTOs;

namespace Service.Services;
public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    private readonly IValidator<UserAddEditDto> _validator;

    public UserService(AppDbContext context, IMapper mapper, IMemoryCache cache, IValidator<UserAddEditDto> validator)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
        _validator = validator;
    }

    public async Task<List<UserGetDto>> GetUsers(int pageNumber, int pageSize)
    {
        var cacheKey = "UserList";

        if (!_cache.TryGetValue(cacheKey, out List<UserGetDto> cachedUsers))
        {
            var users = await _context.Users
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            cachedUsers = _mapper.Map<List<UserGetDto>>(users);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, cachedUsers, cacheOptions);
        }

        return cachedUsers;
    }

    public async Task<List<UserWithProductsDto>> GetUsersWithProducts(int pageNumber, int pageSize)
    {
        var cacheKey = "UserListWithProducts";

        if (!_cache.TryGetValue(cacheKey, out List<UserWithProductsDto> cachedUsersWithProducts))
        {
            var users = await _context.Users
              .AsNoTracking()
              .Include(x => x.UserProducts)
              .Skip((pageNumber - 1) * pageSize)
              .Take(pageSize)
              .ToListAsync();

            cachedUsersWithProducts = _mapper.Map<List<UserWithProductsDto>>(users);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, cachedUsersWithProducts, cacheOptions);
        }

        return cachedUsersWithProducts;
    }

    public async Task<UserGetDto> GetUser(int id)
    {
        var cacheKey = $"User_{id}";

        if (_cache.TryGetValue(cacheKey, out UserGetDto cachedUser))
        {
            return cachedUser;
        }

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (user == null) return null;

        var userDto = _mapper.Map<UserGetDto>(user);

        _cache.Set(cacheKey, userDto, TimeSpan.FromMinutes(5));

        return userDto;
    }

    public async Task<UserWithProductsDto> GetUserWithProducts(int id)
    {
        var cacheKey = $"UserWithProducts_{id}";

        if (_cache.TryGetValue(cacheKey, out UserWithProductsDto cachedUserWithProducts))
        {
            return cachedUserWithProducts;
        }

        var user = await _context.Users
            .AsNoTracking()
            .Include(x => x.UserProducts)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (user == null) return null;

        var userDto = _mapper.Map<UserWithProductsDto>(user);

        _cache.Set(cacheKey, userDto, TimeSpan.FromMinutes(5));

        return userDto;
    }

    public async Task<int> InsertUser(UserAddEditDto userDto)
    {
        var validationResult = await _validator.ValidateAsync(userDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        if (await _context.Users.AnyAsync(x => x.UserName.ToLower().Trim() == userDto.UserName.ToLower().Trim()))
            return -1;

        var user = new User(userDto.UserName, userDto.Email);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Invalidate the cache
        _cache.Remove("UserList");
        _cache.Remove("UserListWithProducts");

        return user.Id;
    }

    public async Task UpdateUser(UserAddEditDto userDto)
    {
        var validationResult = await _validator.ValidateAsync(userDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var user = _mapper.Map<User>(userDto);
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        // Invalidate the cache
        _cache.Remove("UserList");
        _cache.Remove("UserListWithProducts");
    }

    public async Task AssignProductToUser(int userId, int productId)
    {
        var user = await _context.Users.Include(u => u.UserProducts).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) throw new NotFoundException("User not found.");

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        if (product == null) throw new NotFoundException("Product not found.");

        user.AddProduct(product);
        await _context.SaveChangesAsync();

        var cacheKey = $"UserWithProducts_{userId}";
        _cache.Remove(cacheKey);
    }

    public async Task DeleteUser(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null) return;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        // Invalidate the cache
        _cache.Remove("UserList");
        _cache.Remove("UserListWithProducts");
    }
}
