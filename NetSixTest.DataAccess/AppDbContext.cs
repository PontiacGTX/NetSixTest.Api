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
    public DbSet<ProductPicture> Pictures { get; set; }
    public DbSet<ProductsCategories> ProductsCategories { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasKey(x => x.Id);
        modelBuilder.Entity<Category>().Property(x => x.Name).IsRequired();

        modelBuilder.Entity<Product>().HasKey(x=> x.Id);
        modelBuilder.Entity<Product>().Property(x=>x.Name).IsRequired();  
       

        modelBuilder.Entity<ProductPicture>().HasKey(x=>x.ProductPictureId);
        modelBuilder.Entity<ProductPicture>().HasOne(x=>x.Product).WithMany(x=>x.Pictures);

        modelBuilder.Entity<ProductsCategories>()
                                                 .HasOne(pc => pc.Product)
                                                 .WithMany(p => p.ProductsCategories)
                                                 .HasForeignKey(pc => pc.ProductId);

        modelBuilder.Entity<ProductsCategories>()
                                                .HasOne(pc => pc.Category)
                                                .WithMany(c => c.ProductsCategories)
                                                .HasForeignKey(pc => pc.CategoryId);


        base.OnModelCreating(modelBuilder);
    }

}