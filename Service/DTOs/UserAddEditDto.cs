using System.ComponentModel.DataAnnotations;

namespace Service.DTOs;

public record UserAddEditDto
{
    public int? Id { get; set; }
    public string UserName { get; set; }

    public string Email { get; set; }
}

