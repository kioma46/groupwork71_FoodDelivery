using System.ComponentModel.DataAnnotations;

namespace groupwork71.ViewModels;

public class LoginViewModel

{
    [Required]
    [Display(Name = "Enter your Username or Email")]
    public string LoginOrEmail { get; set; }
 
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Display(Name = "Remember?")]
    public bool RememberMe { get; set; }
    public string? ReturnUrl { get; set; }

}