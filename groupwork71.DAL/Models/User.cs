using Microsoft.AspNetCore.Identity;

namespace groupwork71.DAL.Models;

public class User : IdentityUser<int>
{
    public string? ProfilePictureUrl { get; set; }
    public DateTime? DateOfBirth { get; set; }
}