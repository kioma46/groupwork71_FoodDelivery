using groupwork71.BLL.DTOs;
using groupwork71.BLL.Interfaces;
using groupwork71.DAL.DTOs;
using groupwork71.DAL.Models;
using groupwork71.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace groupwork71.Controllers;

public class RestaurantController : Controller
{
    private readonly IRestaurantService _restaurantService;

    public RestaurantController(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
    }

    public async Task<IActionResult> RestaurantList()
    {
        List<Restaurant> restaurants = await _restaurantService.GetAllRestaurantsAsync();
        
        return View(restaurants);
    }
    
    [Authorize(Roles = "Manager")]
    public IActionResult CreateRestaurant()
    {
        return View();
    }
    
    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> CreateRestaurant(CreateRestaurantViewModel model)
    {
        if (ModelState.IsValid)
        {
            var dto = new CreateRestaurantDto
            {
                Name = model.Name,
                Image = model.Image,
                Description = model.Description
            };

            await _restaurantService.CreateRestaurantAsync(dto);
            
            return RedirectToAction(nameof(RestaurantList));
        }
        
        return View(model);
    }
    
    public async Task<IActionResult> RestaurantDetails(int id)
    {
        Restaurant? restaurant = await _restaurantService.GetRestaurantDetailsAsync(id);

        if (restaurant == null)
            return NotFound();

        return View(restaurant);
    }
    
    [HttpPost]
    [Authorize(Roles = "Manager")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteRestaurant(int id)
    {
        bool deleted = await _restaurantService.DeleteRestaurantAsync(id);

        if (!deleted)
            return NotFound();

        return RedirectToAction(nameof(RestaurantList));
    }
    
    [HttpGet]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> EditRestaurant(int id)
    {
        Restaurant? restaurant =
            await _restaurantService.GetRestaurantDetailsAsync(id);

        if (restaurant == null)
            return NotFound();

        var model = new EditRestaurantViewModel
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            Image = restaurant.Image,
            Description = restaurant.Description
        };

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Manager")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRestaurant(EditRestaurantViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        bool updated = await _restaurantService.UpdateRestaurantAsync(
            model.Id,
            new EditRestaurantDto
            {
                Name = model.Name,
                Image = model.Image,
                Description = model.Description
            });

        if (!updated)
            return NotFound();

        return RedirectToAction(nameof(RestaurantDetails), new { id = model.Id });
    }
}