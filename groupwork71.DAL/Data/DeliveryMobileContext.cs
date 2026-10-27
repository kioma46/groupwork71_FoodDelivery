using GroupHomework66.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace groupwork71.DAL.Data;

public class DeliveryMobileContext : IdentityDbContext<User, IdentityRole<int>, int>
{

    public DeliveryMobileContext(DbContextOptions<DeliveryMobileContext> options) : base(options)
    {}
}