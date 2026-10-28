using System.ComponentModel.DataAnnotations;

namespace groupwork71.ViewModels;

public class EditRestaurantViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Укажите название ресторана")]
    [Display(Name = "Название ресторана")]
    public string? Name { get; set; }

    [Display(Name = "Изображение")]
    public string? Image { get; set; }

    [Display(Name = "Описание")]
    public string? Description { get; set; }
}