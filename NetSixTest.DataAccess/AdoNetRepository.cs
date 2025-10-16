
using Microsoft.Data.SqlClient;
using NetSixTest.Data.Entity;
using System.Data;

namespace NetSixTest.DataAccess;

public class AdoNetRepository
{
    private readonly string _connectionString;

    public AdoNetRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Category CRUD
    public async Task<Category?> CreateCategoryAsync(Category category)
    {
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "INSERT INTO Categories ( Name, Enabled) VALUES ( @Name, @Enabled); SELECT SCOPE_IDENTITY();";
            using (SqlCommand command = new(sql, connection))
            {
                command.Parameters.AddWithValue("@Name", category.Name);
                command.Parameters.AddWithValue("@Enabled", category.Enabled);
                var newId = await command.ExecuteScalarAsync();
                if (newId != null)
                {
                    category.Id = Convert.ToInt32(newId);
                    return category;
                }
                return null;
            }
        }
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "SELECT Id, Name, Enabled FROM Categories WHERE Id = @Id;";
            using (SqlCommand command = new(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Category
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Enabled = reader.GetBoolean(reader.GetOrdinal("Enabled"))
                        };
                    }
                }
            }
        }
        return null;
    }

    public async Task<IList<Category>> GetAllCategoriesAsync()
    {
        List<Category> categories = new();
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "SELECT Id, Name, Enabled FROM Categories WHERE Enabled = 1;";
            using (SqlCommand command = new(sql, connection))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        categories.Add(new Category
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Enabled = reader.GetBoolean(reader.GetOrdinal("Enabled"))
                        });
                    }
                }
            }
        }
        return categories;
    }

    public async Task<bool> UpdateCategoryAsync(Category category)
    {
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "UPDATE Categories SET Name = @Name, Enabled = @Enabled WHERE Id = @Id;";
            using (SqlCommand command = new(sql, connection))
            {
                command.Parameters.AddWithValue("@Name", category.Name);
                command.Parameters.AddWithValue("@Enabled", category.Enabled);
                command.Parameters.AddWithValue("@Id", category.Id);
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "UPDATE Categories SET Enabled = 0 WHERE Id = @Id;";
            using (SqlCommand command = new(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }

    public async Task<bool> CategoryExistsAsync(int id)
    {
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "SELECT COUNT(1) FROM Categories WHERE Id = @Id AND Enabled = 1;";
            using (SqlCommand command = new(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                var result = await command.ExecuteScalarAsync();
                return result != null && (int)result > 0;
            }
        }
    }

    public async Task<bool> CategoryExistsByExpressionAsync(string expression, Dictionary<string, object> parameters)
    {
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "SELECT COUNT(1) FROM Categories WHERE " + expression + " AND Enabled = 1;";
            using (SqlCommand command = new(sql, connection))
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }
                var result = await command.ExecuteScalarAsync();
                return result != null && (int)result > 0;
            }
        }
    }


    public async Task<Product?> CreateProductAsync(Product product)
    {
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "INSERT INTO Products ( Name, Price, Quantity, Enabled) VALUES (@Name, @Price, @Quantity, @Enabled); SELECT SCOPE_IDENTITY();";
            using (SqlCommand command = new(sql, connection))
            {
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Quantity", product.Quantity); 
                command.Parameters.AddWithValue("@Enabled", product.Enabled);
                var newId = await command.ExecuteScalarAsync();
                if (newId != null)
                {
                    product.Id = Convert.ToInt32(newId);

                    if (product.ProductsCategories != null && product.ProductsCategories.Any())
                    {
                        foreach (var pc in product.ProductsCategories)
                        {
                            string insertProductCategorySql = "INSERT INTO ProductsCategories (ProductId, CategoryId) VALUES (@ProductId, @CategoryId);";
                            using (SqlCommand pcCommand = new(insertProductCategorySql, connection))
                            {
                                pcCommand.Parameters.AddWithValue("@ProductId", product.Id);
                                pcCommand.Parameters.AddWithValue("@CategoryId", pc.CategoryId);
                                await pcCommand.ExecuteNonQueryAsync();
                            }
                        }
                    }
                    return product;
                }
                return null;
            }
        }
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "SELECT p.Id, p.Name, p.Price, p.Quantity, p.Enabled, pc.CategoryId, c.Name AS CategoryName, c.Enabled AS CategoryEnabled FROM Products p LEFT JOIN ProductsCategories pc ON p.Id = pc.ProductId LEFT JOIN Categories c ON pc.CategoryId = c.Id WHERE p.Id = @Id;";
            using (SqlCommand command = new(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    Product? product = null;
                    Dictionary<int, Category> categories = new Dictionary<int, Category>();

                    while (await reader.ReadAsync())
                    {
                        if (product == null)
                        {
                            product = new Product
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Price = reader.GetDouble(reader.GetOrdinal("Price")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                Enabled = reader.GetBoolean(reader.GetOrdinal("Enabled")),
                                ProductsCategories = new List<ProductsCategories>()
                            };
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("CategoryId")))
                        {
                            int categoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"));
                            if (!categories.ContainsKey(categoryId))
                            {
                                var category = new Category
                                {
                                    Id = categoryId,
                                    Name = reader.GetString(reader.GetOrdinal("CategoryName")),
                                    Enabled = reader.GetBoolean(reader.GetOrdinal("CategoryEnabled"))
                                };
                                categories.Add(categoryId, category);
                                product.ProductsCategories.Add(new ProductsCategories { Product = product, Category = category, ProductId = product.Id, CategoryId = category.Id });
                            }
                        }
                    }
                    return product;
                }
            }
        }
    }

    public async Task<IList<Product>> GetAllProductsAsync()
    {
        List<Product> products = new();
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "SELECT p.Id, p.Name, p.Price, p.Quantity, p.Enabled, pc.CategoryId, c.Name AS CategoryName, c.Enabled AS CategoryEnabled FROM Products p LEFT JOIN ProductsCategories pc ON p.Id = pc.ProductId LEFT JOIN Categories c ON pc.CategoryId = c.Id WHERE p.Enabled = 1;";
            using (SqlCommand command = new(sql, connection))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    Dictionary<int, Product> productMap = new();

                    while (await reader.ReadAsync())
                    {
                        int productId = reader.GetInt32(reader.GetOrdinal("Id"));
                        if (!productMap.ContainsKey(productId))
                        {
                            productMap.Add(productId, new Product
                            {
                                Id = productId,
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Price = reader.GetDouble(reader.GetOrdinal("Price")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                Enabled = reader.GetBoolean(reader.GetOrdinal("Enabled")),
                                ProductsCategories = new List<ProductsCategories>()
                            });
                        }

                        Product currentProduct = productMap[productId];

                        if (!reader.IsDBNull(reader.GetOrdinal("CategoryId")))
                        {
                            int categoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"));
                            // Check if this specific ProductCategory relationship already exists for the current product
                            var productCategories = currentProduct.ProductsCategories ?? new List<ProductsCategories>();
                            if (!productCategories!.Any(pc => pc.CategoryId == categoryId))
                            {
                                var category = new Category
                                {
                                    Id = categoryId,
                                    Name = reader.GetString(reader.GetOrdinal("CategoryName")),
                                    Enabled = reader.GetBoolean(reader.GetOrdinal("CategoryEnabled"))
                                };
                                currentProduct.ProductsCategories.Add(new ProductsCategories { Product = currentProduct, Category = category, ProductId = currentProduct.Id, CategoryId = category.Id });
                            }
                        }
                    }
                    products.AddRange(productMap.Values);
                }
            }
        }
        return products;
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "UPDATE Products SET Name = @Name, Price = @Price, Quantity = @Quantity, Enabled = @Enabled WHERE Id = @Id;";
            using (SqlCommand command = new(sql, connection))
            {
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                command.Parameters.AddWithValue("@Enabled", product.Enabled);
                command.Parameters.AddWithValue("@Id", product.Id);
                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    // Eliminar las categorías existentes para el producto
                    string deletePcSql = "DELETE FROM ProductsCategories WHERE ProductId = @ProductId;";
                    using (SqlCommand deletePcCommand = new(deletePcSql, connection))
                    {
                        deletePcCommand.Parameters.AddWithValue("@ProductId", product.Id);
                        await deletePcCommand.ExecuteNonQueryAsync();
                    }

                    // Insertar las nuevas categorías
                    var productCategories = product.ProductsCategories ?? new List<ProductsCategories>();
                    if (productCategories!.Any())
                    {
                        foreach (var pc in product.ProductsCategories)
                        {
                            string insertProductCategorySql = "INSERT INTO ProductsCategories (ProductId, CategoryId) VALUES (@ProductId, @CategoryId);";
                            using (SqlCommand pcCommand = new(insertProductCategorySql, connection))
                            {
                                pcCommand.Parameters.AddWithValue("@ProductId", product.Id);
                                pcCommand.Parameters.AddWithValue("@CategoryId", pc.CategoryId);
                                await pcCommand.ExecuteNonQueryAsync();
                            }
                        }
                    }
                    return true;
                }
                return false;
            }
        }
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "UPDATE Products SET Enabled = 0 WHERE Id = @Id;";
            using (SqlCommand command = new(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    // Eliminar las categorías relacionadas en ProductsCategories
                    string deletePcSql = "DELETE FROM ProductsCategories WHERE ProductId = @ProductId;";
                    using (SqlCommand deletePcCommand = new(deletePcSql, connection))
                    {
                        deletePcCommand.Parameters.AddWithValue("@ProductId", id);
                        await deletePcCommand.ExecuteNonQueryAsync();
                    }
                    return true;
                }
                return false;
            }
        }
    }

    public async Task<bool> ProductExistsAsync(int id)
    {
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "SELECT COUNT(1) FROM Products WHERE Id = @Id AND Enabled = 1;";
            using (SqlCommand command = new(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                var result = await command.ExecuteScalarAsync();
                return result != null && (int)result > 0;
            }
        }
    }

    public async Task<bool> ProductExistsByExpressionAsync(string expression, Dictionary<string, object> parameters)
    {
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "SELECT COUNT(1) FROM Products WHERE " + expression + " AND Enabled = 1;";
            using (SqlCommand command = new(sql, connection))
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }
                var result = await command.ExecuteScalarAsync();
                return result != null && (int)result > 0;
            }
        }
    }

    // ProductsCategories CRUD
    public async Task<ProductsCategories?> CreateProductsCategoriesAsync(ProductsCategories productsCategories)
    {
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "INSERT INTO ProductsCategories (ProductId, CategoryId) VALUES (@ProductId, @CategoryId); SELECT SCOPE_IDENTITY();";
            using (SqlCommand command = new(sql, connection))
            {
                command.Parameters.AddWithValue("@ProductId", productsCategories.ProductId);
                command.Parameters.AddWithValue("@CategoryId", productsCategories.CategoryId);
                var newId = await command.ExecuteScalarAsync();
                if (newId != null)
                {
                    // ProductsCategories uses Guid as Id, so we need to convert it
                    string newIdString = newId.ToString()!;
                    if (Guid.TryParse(newIdString, out Guid parsedGuid))
                    {
                        productsCategories.Id = parsedGuid;
                    }
                    else
                    {
                        productsCategories.Id = null; // O manejar el error de otra manera
                    }
                    return productsCategories;
                }
                return null;
            }
        }
    }

    public async Task<ProductsCategories?> GetProductsCategoriesByIdAsync(Guid id)
    {
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "SELECT Id, ProductId, CategoryId FROM ProductsCategories WHERE Id = @Id;";
            using (SqlCommand command = new(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new ProductsCategories
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                            CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"))
                        };
                    }
                }
            }
        }
        return null;
    }

    public async Task<IList<ProductsCategories>> GetAllProductsCategoriesAsync()
    {
        List<ProductsCategories> productsCategories = new();
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "SELECT Id, ProductId, CategoryId FROM ProductsCategories;";
            using (SqlCommand command = new(sql, connection))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        productsCategories.Add(new ProductsCategories
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                            CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"))
                        });
                    }
                }
            }
        }
        return productsCategories;
    }

    public async Task<bool> UpdateProductsCategoriesAsync(ProductsCategories productsCategories)
    {
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "UPDATE ProductsCategories SET ProductId = @ProductId, CategoryId = @CategoryId WHERE Id = @Id;";
            using (SqlCommand command = new(sql, connection))
            {
                command.Parameters.AddWithValue("@ProductId", productsCategories.ProductId);
                command.Parameters.AddWithValue("@CategoryId", productsCategories.CategoryId);
                command.Parameters.AddWithValue("@Id", productsCategories.Id);
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }

    public async Task<bool> DeleteProductsCategoriesAsync(Guid id)
    {
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "DELETE FROM ProductsCategories WHERE Id = @Id;";
            using (SqlCommand command = new(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }

    public async Task<bool> ProductsCategoriesExistsAsync(Guid id)
    {
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "SELECT COUNT(1) FROM ProductsCategories WHERE Id = @Id;";
            using (SqlCommand command = new(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                var result = await command.ExecuteScalarAsync();
                return result != null && (int)result > 0;
            }
        }
    }
}

