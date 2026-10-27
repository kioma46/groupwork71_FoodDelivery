using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace groupwork71.ViewModels;

public class RegisterViewModel

{
    [Required]
    [MinLength(3)]
    [Remote("ValidateUserName", "Validation", ErrorMessage = "Username occupied")]
    public string? UserName { get; set; }
    
    [Required]
    [Display(Name = "Email")]
    [EmailAddress]
    [Remote("ValidateEmail", "Validation", ErrorMessage = "Address occupied")]
    public string Email { get; set; }

    [Required]
    [MinLength(3)]
    public string ProfilePictureUrl { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    [Remote("ValidatePassword", "Validation", ErrorMessage = "Incorrect password (it must contain at least 8 characters, 1 digit and 1 symbol!)")]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    public string PasswordConfirm { get; set; }
    
    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    [Remote("ValidateDateOfBirth", "Validation", ErrorMessage = "You must be at least 18 years old")]
    public DateTime DateOfBirth { get; set; }
    
}