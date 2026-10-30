using System.Security.Claims;
using groupwork71.BLL.Interfaces;
using groupwork71.BLL.Services;
using groupwork71.DAL.Models;
using groupwork71.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace groupwork71.Controllers;

[Authorize]
public class OrderController : Controller
{
    
    private readonly UserManager<User> _userManager;
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;
    private readonly EmailService _emailService;

    public OrderController(IOrderService orderService, ICartService cartService,  EmailService emailService, UserManager<User> userManager)
    {
        _orderService = orderService;
        _cartService = cartService;
        _emailService = emailService;
        _userManager = userManager;
    }

    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    
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
            var cu = await _userManager.FindByIdAsync(CurrentUserId.ToString());
            if (cu != null && cu.Email != null)
            {
                string currentUserEmail = cu.Email;
                
                await _emailService.SendEmailAsync(currentUserEmail, "Successful order", CreateEmail(order));
                return RedirectToAction("Success", new { id = order.Id });
            }
            return BadRequest(ModelState);
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

    [Authorize(Roles = "Admin, Manager")]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await _orderService.GetAllAsync());
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _orderService.DeleteByIdAsync(id);
        return RedirectToAction("Index", "Order");
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        return View(order);
    }

    private string CreateEmail(Order order)
    {
        var itemsHtml = new System.Text.StringBuilder();
        foreach (var oi in order.OrderItems)
        {
            itemsHtml.Append($"<li>{oi.Dish?.Name} — {oi.Quantity} шт. по {oi.Price:0.00} (Сумма: {(oi.Price * oi.Quantity):0.00})</li>");
        }

        string emailText = $"<h4>Вы успешно оформили заказ №{order.Id}!</h4>" +
                           $"<p><strong>Получатель:</strong> {order.RecipientName}</p>" +
                           $"<p><strong>Адрес доставки:</strong> {order.DeliveryAddress}</p>" +
                           $"<p><strong>Номер телефона:</strong> {order.PhoneNumber}</p>" +
                           $"<p><strong>Дата оформления:</strong> {order.CreatedAt?.ToString("dd.MM.yyyy HH:mm")}</p>" +
                           $"<p><strong>Состав заказа:</strong></p>" +
                           $"<ul>{itemsHtml}</ul>" +
                           $"<p><strong>Итоговая цена:</strong> {order.TotalPrice:0.00}</p>";

        return emailText;
    }
}