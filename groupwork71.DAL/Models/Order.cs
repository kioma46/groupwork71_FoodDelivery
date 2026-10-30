namespace groupwork71.DAL.Models;

public class Order
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }
    
    public int RestaurantId { get; set; }
    public Restaurant? Restaurant { get; set; }
    
    public string RecipientName { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string DeliveryAddress { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    
    public decimal TotalPrice { get; set; }
    
    public List<OrderItem> OrderItems { get; set; } = new ();
}