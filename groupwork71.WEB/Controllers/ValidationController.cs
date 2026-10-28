using groupwork71.DAL.Data;
using groupwork71.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace groupwork71.Controllers;

public class ValidationController : Controller
{
    private DeliveryMobileContext _context;
    private readonly UserManager<User> _userManager;

    public ValidationController(UserManager<User> userManager, DeliveryMobileContext context)
    {
        _context = context;
        _userManager = userManager;
    }
    
    public bool ValidatePhoneNumber(string PhoneNumber) => PhoneNumber.All(char.IsDigit);
    
    public bool ValidateEmail(string Email) => !_userManager.Users.Any(u => u.Email == Email);
    
    public bool ValidateUserName(string UserName) => !_userManager.Users.Any(u => u.UserName == UserName);
    
    public bool ValidatePassword(string Password) => Password.Length >= 8 && Password.Any(char.IsDigit) && Password.Any(char.IsUpper) && Password.Any(c => !char.IsLetter(c));

    public bool ValidateDateOfBirth(DateTime DateOfBirth) => DateOfBirth <= DateTime.UtcNow.AddYears(-18);
}