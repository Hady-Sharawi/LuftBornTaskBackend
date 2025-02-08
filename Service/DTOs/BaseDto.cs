namespace Service.DTOs;
public abstract record BaseDto
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
}
