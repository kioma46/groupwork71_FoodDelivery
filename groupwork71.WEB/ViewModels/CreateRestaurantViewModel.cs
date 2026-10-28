using System.ComponentModel.DataAnnotations;

namespace groupwork71.ViewModels;

public class CreateRestaurantViewModel
{
    [Required(ErrorMessage = "Укажите название ресторана")]
    [Display(Name = "Название ресторана")]
    public string? Name { get; set; }
    
    [Display(Name = "Изображение")]
    public string? Image { get; set; }
    
    [Display(Name = "Описание")]
    public string? Description { get; set; }
}