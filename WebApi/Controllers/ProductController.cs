using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Contracts;
using Service.DTOs;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    // api/product
    [HttpGet]
    public async Task<IActionResult> GetProducts(int pageNumber = 1, int pageSize = 10)
    {
        var products = await _productService.GetProducts(pageNumber, pageSize);
        return Ok(products);
    }

    // api/product/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _productService.GetProduct(id);

        if (product == null)
            throw new NotFoundException("Product not found");

        return Ok(product);
    }

    // api/product
    [HttpPost]
    public async Task<IActionResult> InsertProduct([FromForm] ProductAddEditDto productDto)
    {
        if (productDto == null)
            throw new BadRequestException("Product data is null");

        int productId = await _productService.InsertProduct(productDto);

        return Ok(productId);
    }

    // api/product
    [HttpPut]
    public async Task<IActionResult> UpdateProduct([FromForm] ProductAddEditDto productDto)
    {
        if (productDto == null)
            throw new BadRequestException("Product data is null");

        await _productService.UpdateProduct(productDto);

        return Ok();
    }

    // api/product/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _productService.DeleteProduct(id);

        return Ok();
    }
}
