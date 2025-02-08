namespace Service.DTOs;
public record ProductWithUserDto
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public UserGetDto User { get; set; }
}
