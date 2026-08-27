using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Api.Models;

namespace Ecommerce.Api.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }


    public DbSet<Producto> Productos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Carrito> Carritos { get; set; }
    public DbSet<CarritoItem> CarritoItems { get; set; }
    public DbSet<ProductoVariante> ProductoVariantes { get; set; }
    public DbSet<Orden> Ordenes { get; set; }
    public DbSet<OrdenItem> OrdenItems { get; set; }
    public DbSet<CuponDescuento> Cupones { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

}