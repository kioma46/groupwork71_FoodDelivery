namespace groupwork71.DAL.Models;

public class Restaurant
{
    public int Id { get; set; }
    
    public string? Name { get; set; }
    
    public string? Image { get; set; }
    
    public string? Description { get; set; }
    
    public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}