using Microsoft.AspNetCore.Identity;

namespace GroupHomework66.Models;

public class User : IdentityUser<int>
{
    public string? ProfilePictureUrl { get; set; }
    public DateTime? DateOfBirth { get; set; }
}