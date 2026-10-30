using System.Security.Claims;
using groupwork71.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace groupwork71.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> Sidebar(int restaurantId)
    {
        var cart = await _cartService.GetCartAsync(CurrentUserId, restaurantId);
        return PartialView("_CartSidebar", cart);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int dishId, int restaurantId)
    {
        bool added = await _cartService.AddDishAsync(CurrentUserId, restaurantId, dishId);
        if (!added)
            return NotFound();

        return await Sidebar(restaurantId);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Decrease(int dishId, int restaurantId)
    {
        await _cartService.DecreaseDishAsync(CurrentUserId, dishId);
        return await Sidebar(restaurantId);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int dishId, int restaurantId)
    {
        await _cartService.RemoveDishAsync(CurrentUserId, dishId);
        return await Sidebar(restaurantId);
    }

    public async Task<IActionResult> Clear(int restaurantId)
    {
        await _cartService.ClearCartAsync(CurrentUserId, restaurantId);
        return await Sidebar(restaurantId);
    }
}