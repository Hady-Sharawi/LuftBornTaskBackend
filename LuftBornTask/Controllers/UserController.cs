using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Contracts;
using Service.DTOs;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // api/user
    [HttpGet]
    public async Task<IActionResult> GetUsers(int pageNumber = 1, int pageSize = 10)
    {
        var users = await _userService.GetUsers(pageNumber, pageSize);
        return Ok(users);
    }

    // api/user/products
    [HttpGet("products")]
    public async Task<IActionResult> GetUsersWithProducts(int pageNumber = 1, int pageSize = 10)
    {
        var usersWithProducts = await _userService.GetUsersWithProducts(pageNumber, pageSize);

        return Ok(usersWithProducts);
    }

    // api/user/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _userService.GetUser(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    // api/user/products/{id}
    [HttpGet("products/{id}")]
    public async Task<IActionResult> GetUserWithProducts(int id)
    {
        var userWithProducts = await _userService.GetUserWithProducts(id);

        if (userWithProducts == null)
            return NotFound();

        return Ok(userWithProducts);
    }

    // api/user
    [HttpPost]
    public async Task<IActionResult> InsertUser([FromForm] UserAddEditDto userDto)
    {
        if (userDto == null)
            throw new BadRequestException("User data is null");

        int userId = await _userService.InsertUser(userDto);

        if (userId == -1)
            throw new BadRequestException("User Name already exists.");

        return Ok(userId);
    }

    // api/user
    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromForm] UserAddEditDto userDto)
    {
        if (userDto == null)
            throw new BadRequestException("User data is null");

        await _userService.UpdateUser(userDto);

        return Ok();
    }

    // api/user/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userService.DeleteUser(id);

        return Ok();
    }

    // api/user/{userId}/product/{productId}
    [HttpPost("{userId}/product/{productId}")]
    public async Task<IActionResult> AssignProductToUser(int userId, int productId)
    {
        await _userService.AssignProductToUser(userId, productId);
        return Ok();
    }
}
