
using groupwork71.BLL.DTOs;
using groupwork71.BLL.Interfaces;
using groupwork71.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace groupwork71.Controllers;

[Authorize(Roles = "Admin,Manager")]
public class DishController : Controller
{
    private readonly IDishService _dishService;
    private readonly IRestaurantService _restaurantService;

    public DishController(IDishService dishService, IRestaurantService restaurantService)
    {
        _dishService = dishService;
        _restaurantService = restaurantService;
    }

    [HttpGet]
    public async Task<IActionResult> Create(int? restaurantId)
    {
        var restaurants = await _restaurantService.GetAllRestaurantsAsync();
        if (restaurantId.HasValue && !restaurants.Any(r => r.Id == restaurantId.Value))
        {
            return NotFound();
        }

        await LoadRestaurantsAsync(restaurantId);

        return View(new CreateDishViewModel { RestaurantId = restaurantId ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateDishViewModel model)
    {
        var restaurants = await _restaurantService.GetAllRestaurantsAsync();
        if (!restaurants.Any(r => r.Id == model.RestaurantId))
        {
            ModelState.AddModelError(nameof(model.RestaurantId), "Выберите существующее заведение.");
        }

        if (!ModelState.IsValid)
        {
            await LoadRestaurantsAsync(model.RestaurantId);
            return View(model);
        }

        bool created = await _dishService.CreateAsync(new CreateDishDto
        {
            Name = model.Name,
            Price = model.Price,
            Description = model.Description,
            RestaurantId = model.RestaurantId
        });

        if (!created)
        {
            ModelState.AddModelError(string.Empty, "Не удалось добавить блюдо. Проверьте введённые данные.");

            await LoadRestaurantsAsync(model.RestaurantId);
            return View(model);
        }

        return RedirectToAction("RestaurantDetails", "Restaurant", new { id = model.RestaurantId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int restaurantId)
    {
        bool deleted = await _dishService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return RedirectToAction("RestaurantDetails", "Restaurant", new { id = restaurantId });
    }

    private async Task LoadRestaurantsAsync(int? selectedId)
    {
        var restaurants = await _restaurantService.GetAllRestaurantsAsync();

        ViewBag.Restaurants = new SelectList(restaurants, "Id", "Name", selectedId);
    }
}
