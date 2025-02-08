using System.ComponentModel.DataAnnotations;

namespace Service.DTOs;
public record UserGetDto : BaseDto
{
    public string UserName { get; set; }

    public string Email { get; set; }

}
