namespace Service.DTOs;
public record UserWithProductsDto : BaseDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public List<ProductGetDto> UserProducts { get; set; }
}
