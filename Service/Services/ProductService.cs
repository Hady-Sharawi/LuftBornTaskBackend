using AutoMapper;
using Entities.Domains;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Repository;
using Service.Contracts;
using Service.DTOs;

namespace Service.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    private readonly IValidator<ProductAddEditDto> _validator;

    public ProductService(AppDbContext context, IMapper mapper, IMemoryCache cache, IValidator<ProductAddEditDto> validator)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
        _validator = validator;
    }

    public async Task<List<ProductGetDto>> GetProducts(int pageNumber, int pageSize)
    {
        var cacheKey = "ProductList";

        if (!_cache.TryGetValue(cacheKey, out List<ProductGetDto> cachedProducts))
        {
            var products = await _context.Products
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            cachedProducts = _mapper.Map<List<ProductGetDto>>(products);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, cachedProducts, cacheOptions);
        }

        return cachedProducts;
    }

    public async Task<List<ProductWithUserDto>> GetProductsWithUser(int pageNumber, int pageSize)
    {
        var cacheKey = "ProductListWithUsers";

        if (!_cache.TryGetValue(cacheKey, out List<ProductWithUserDto> cachedProductsWithUsers))
        {
            var products = await _context.Products
                .AsNoTracking()
                .Include(p => p.User)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            cachedProductsWithUsers = _mapper.Map<List<ProductWithUserDto>>(products);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, cachedProductsWithUsers, cacheOptions);
        }

        return cachedProductsWithUsers;
    }

    public async Task<ProductGetDto> GetProduct(int id)
    {
        var cacheKey = $"Product_{id}";

        if (_cache.TryGetValue(cacheKey, out ProductGetDto cachedProduct))
        {
            return cachedProduct;
        }

        var product = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return null;

        var productDto = _mapper.Map<ProductGetDto>(product);

        _cache.Set(cacheKey, productDto, TimeSpan.FromMinutes(5));

        return productDto;
    }

    public async Task<ProductWithUserDto> GetProductWithUser(int id)
    {
        var cacheKey = $"ProductWithUser_{id}";

        if (_cache.TryGetValue(cacheKey, out ProductWithUserDto cachedProductWithUser))
        {
            return cachedProductWithUser;
        }

        var product = await _context.Products
            .AsNoTracking()
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return null;

        var productDto = _mapper.Map<ProductWithUserDto>(product);

        _cache.Set(cacheKey, productDto, TimeSpan.FromMinutes(5));

        return productDto;
    }

    public async Task<int> InsertProduct(ProductAddEditDto productDto)
    {
        var validationResult = await _validator.ValidateAsync(productDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var product = new Product(productDto.Name);
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        // Invalidate the cache
        _cache.Remove("ProductList");
        _cache.Remove("ProductListWithUsers");

        return product.Id;
    }

    public async Task UpdateProduct(ProductAddEditDto productDto)
    {
        var validationResult = await _validator.ValidateAsync(productDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var product = _mapper.Map<Product>(productDto);
        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        // Invalidate the cache
        _cache.Remove("ProductList");
        _cache.Remove("ProductListWithUsers");
    }

    public async Task DeleteProduct(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        // Invalidate the cache
        _cache.Remove("ProductList");
        _cache.Remove("ProductListWithUsers");
    }
}
