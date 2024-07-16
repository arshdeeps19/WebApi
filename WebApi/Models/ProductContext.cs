using System.Data.Entity;
using WebApi.Models;

namespace WebApi.Models
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
    }
}
