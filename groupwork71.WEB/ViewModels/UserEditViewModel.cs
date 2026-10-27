using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace groupwork71.ViewModels;

public class UserEditViewModel
{
    public int Id { get; set; }

    [Required]
    [MinLength(3)]
    [Remote("ValidateUserName", "Validation", ErrorMessage = "Username occupied", AdditionalFields =  nameof(Id))]
    public string? UserName { get; set; }
    
    [Required]
    [Display(Name = "Email")]
    [EmailAddress]
    [Remote("ValidateEmail", "Validation", ErrorMessage = "Address occupied",  AdditionalFields =  nameof(Id))]
    public string Email { get; set; }

    [Required]
    [MinLength(3)]
    public string ProfilePictureUrl { get; set; }
    
    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    [Remote("ValidateDateOfBirth", "Validation", ErrorMessage = "You must be at least 18 years old")]
    public DateTime DateOfBirth { get; set; }
}