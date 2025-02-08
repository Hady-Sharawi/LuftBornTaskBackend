namespace Service.DTOs;
public record ProductGetDto : BaseDto
{
    public string Name { get; set; }
}
