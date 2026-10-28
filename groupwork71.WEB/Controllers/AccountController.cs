using groupwork71.DAL.Models;
using groupwork71.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace groupwork71.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    
    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    private int CurrentUserId => int.Parse(_userManager.GetUserId(User)!);
    private bool IsAdmin => User.IsInRole("Admin");
    // GET
    
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users.ToList();
        var model = new List<UserManagementViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            model.Add(new UserManagementViewModel
            {
                User = user,
                Roles = roles,
                SelectedRole = roles.FirstOrDefault() ?? "User",
                IsCurrentUser = user.Id == CurrentUserId
            });
        }

        return View(model);
    }

    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            model.DateOfBirth = DateTime.SpecifyKind(model.DateOfBirth, DateTimeKind.Utc);
        
            User user = new User { 
                UserName =  model.UserName,
                Email = model.Email,
                ProfilePictureUrl =  model.ProfilePictureUrl,
                DateOfBirth = model.DateOfBirth
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await  _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }
        return View(model);
    }
    
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var loginOrEmail = model.LoginOrEmail?.Trim();

        if (loginOrEmail != null)
        {
            User? user = await _userManager.FindByEmailAsync(loginOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(loginOrEmail);
            }

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login or password");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName!, 
                model.Password, 
                model.RememberMe, 
                lockoutOnFailure: true
            );

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "User account is locked out.");
                return View(model);
            }
        }

        ModelState.AddModelError(string.Empty, "Invalid login or password");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> LogOff()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    [HttpGet]
    [Authorize]
    public IActionResult Edit(int? userId)
    {
        if (userId != null)
        {
            if (userId != CurrentUserId && !IsAdmin)
                return Forbid();
            
            User? user = _userManager.Users.FirstOrDefault(u => u.Id == userId.Value);
            if (user != null)
            {
                UserEditViewModel uevm = new()
                {
                    Id = user.Id,
                    Email = user.Email!,
                    UserName = user.UserName,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                    DateOfBirth = user.DateOfBirth.Value,
                };
                return View(uevm);
            }
        }
        
        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Edit(UserEditViewModel uevm)
    {
        if (ModelState.IsValid)
        {
            User? user = _userManager.Users.FirstOrDefault(u => u.Id == uevm.Id);

            if (user != null)
            {
                uevm.DateOfBirth = DateTime.SpecifyKind(uevm.DateOfBirth, DateTimeKind.Utc);
                
                user.UserName = uevm.UserName;
                user.Email = uevm.Email;
                user.DateOfBirth = uevm.DateOfBirth;
                user.ProfilePictureUrl = uevm.ProfilePictureUrl;

                user.NormalizedUserName = _userManager.KeyNormalizer.NormalizeName(uevm.UserName);
                user.NormalizedEmail = _userManager.KeyNormalizer.NormalizeEmail(uevm.Email);

                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                
                return View(uevm);
            }
            
            return NotFound();
        }
        
        return View(uevm);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> ChangeRole(int userId, string role)
    {
        if (role != "Admin" && role != "User")
        {
            TempData["Error"] = "Недопустимая роль.";
            return RedirectToAction(nameof(Index));
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
            return NotFound();

        if (user.Id == CurrentUserId)
        {
            TempData["Error"] = "Нельзя изменять собственную роль.";
            return RedirectToAction(nameof(Index));
        }

        var currentRoles = await _userManager.GetRolesAsync(user);

        if (currentRoles.Contains("Manager"))
        {
            TempData["Error"] = "Нельзя изменять роль главного управляющего.";
            return RedirectToAction(nameof(Index));
        }

        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

        if (!removeResult.Succeeded)
        {
            TempData["Error"] = "Не удалось удалить старые роли.";
            return RedirectToAction(nameof(Index));
        }

        var addResult = await _userManager.AddToRoleAsync(user, role);

        if (!addResult.Succeeded)
        {
            TempData["Error"] = "Не удалось назначить новую роль.";
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = $"Пользователю {user.UserName} назначена роль {role}.";

        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
            return NotFound();

        if (user.Id == CurrentUserId)
        {
            TempData["Error"] = "Нельзя удалить собственный аккаунт.";
            return RedirectToAction(nameof(Index));
        }

        var roles = await _userManager.GetRolesAsync(user);

        if (roles.Contains("Manager"))
        {
            TempData["Error"] = "Нельзя удалить главного управляющего.";
            return RedirectToAction(nameof(Index));
        }

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            TempData["Error"] = string.Join("; ", result.Errors.Select(e => e.Description));

            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = $"Пользователь {user.UserName} удалён.";

        return RedirectToAction(nameof(Index));
    }
    
    
    [HttpGet]
    [Authorize(Roles = "Manager")]
    public IActionResult CreateUser()
    {
        return View(new CreateUserViewModel());
    }

    [HttpPost]
    [Authorize(Roles = "Manager")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(CreateUserViewModel model)
    {
        if (model.Role != "User" && model.Role != "Admin")
        {
            ModelState.AddModelError(nameof(model.Role), "Недопустимая роль.");
        }

        if (!ModelState.IsValid)
            return View(model);

        var user = new User
        {
            UserName = model.UserName,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        var roleResult = await _userManager.AddToRoleAsync(user, model.Role);

        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);

            foreach (var error in roleResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        TempData["Success"] = $"Пользователь {user.UserName} успешно создан.";

        return RedirectToAction(nameof(Index));
    }
}