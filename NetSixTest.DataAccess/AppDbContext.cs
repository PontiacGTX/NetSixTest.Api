using Microsoft.EntityFrameworkCore;
using NetSixTest.Data.Entity;

namespace NetSixTest.Data;

public class AppDbContext:DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
	{


	}
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().Property(x => x.Name).IsRequired();

        modelBuilder.Entity<Product>().HasKey(x=> x.Id);
        modelBuilder.Entity<Product>().Property(x=>x.Name).IsRequired();
        modelBuilder.Entity<Product>().HasOne(x => x.Category).WithMany(x => x.Products);


        base.OnModelCreating(modelBuilder);
    }

}