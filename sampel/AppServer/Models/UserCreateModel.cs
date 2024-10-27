using System.ComponentModel.DataAnnotations;

namespace AppServer.Models;

public class UserCreateModel
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Surname { get; set; }

    [Required]
    [EmailAddress]
    public string Mail { get; set; }

    [Required]
    public string Country { get; set; }
}