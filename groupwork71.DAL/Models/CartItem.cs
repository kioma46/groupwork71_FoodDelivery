namespace groupwork71.DAL.Models;

public class CartItem
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    public int DishId { get; set; }
    public Dish? Dish { get; set; }

    public int Quantity { get; set; }
}