using System.Security.Claims;
using groupwork71.BLL.Interfaces;
using groupwork71.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace groupwork71.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;

    public OrderController(IOrderService orderService, ICartService cartService)
    {
        _orderService = orderService;
        _cartService = cartService;
    }

    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateOrderViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("RestaurantDetails", "Restaurant", new { id = model.RestaurantId });
        }

        var cart = await _cartService.GetCartAsync(CurrentUserId, model.RestaurantId);

        if (!cart.Items.Any())
        {
            TempData["Error"] = "Ваша корзина пуста.";
            return RedirectToAction("RestaurantDetails", "Restaurant", new { id = model.RestaurantId });
        }

        try
        {
            var order = await _orderService.CreateOrderAsync(
                CurrentUserId,
                model.RestaurantId,
                model.RecipientName!,
                model.PhoneNumber!,
                model.DeliveryAddress!,
                cart
            );

            return RedirectToAction("Success", new { id = order.Id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction("RestaurantDetails", "Restaurant", new { id = model.RestaurantId });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Success(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        return View(order);
    }
}