using System.ComponentModel.DataAnnotations;

namespace groupwork71.ViewModels;

public class CreateDishViewModel
{
    [Required]
    public string? Name { get; set; }

    [Range(0.01, 1000000)]
    public decimal Price { get; set; }

    public string? Description { get; set; }

    [Range(1, int.MaxValue)]
    public int RestaurantId { get; set; }
}