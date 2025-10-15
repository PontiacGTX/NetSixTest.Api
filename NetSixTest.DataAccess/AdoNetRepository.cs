
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
                return (int)await command.ExecuteScalarAsync() > 0;
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
                return (int)await command.ExecuteScalarAsync() > 0;
            }
        }
    }


    public async Task<Product?> CreateProductAsync(Product product)
    {
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "INSERT INTO Products ( Name, Price, Quantity, CategoryId, Enabled) VALUES (@Name, @Price, @Quantity, @CategoryId, @Enabled); SELECT SCOPE_IDENTITY();";
            using (SqlCommand command = new(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", product.Id);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                command.Parameters.AddWithValue("@Enabled", product.Enabled);
                var newId = await command.ExecuteScalarAsync();
                if (newId != null)
                {
                    product.Id = Convert.ToInt32(newId);
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
            string sql = "SELECT p.Id, p.Name, p.Price, p.Quantity, p.CategoryId, p.Enabled, c.Id AS CategoryId, c.Name AS CategoryName, c.Enabled AS CategoryEnabled FROM Products p JOIN Categories c ON p.CategoryId = c.Id WHERE p.Id = @Id;";
            using (SqlCommand command = new(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Product
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Price = reader.GetDouble(reader.GetOrdinal("Price")),
                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                            CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                            Enabled = reader.GetBoolean(reader.GetOrdinal("Enabled")),
                            Category = new Category
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                Name = reader.GetString(reader.GetOrdinal("CategoryName")),
                                Enabled = reader.GetBoolean(reader.GetOrdinal("CategoryEnabled"))
                            }
                        };
                    }
                }
            }
        }
        return null;
    }

    public async Task<IList<Product>> GetAllProductsAsync()
    {
        List<Product> products = new();
        using (SqlConnection connection = new(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "SELECT p.Id, p.Name, p.Price, p.Quantity, p.CategoryId, p.Enabled, c.Id AS CategoryId, c.Name AS CategoryName, c.Enabled AS CategoryEnabled FROM Products p JOIN Categories c ON p.CategoryId = c.Id WHERE p.Enabled = 1;";
            using (SqlCommand command = new(sql, connection))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        products.Add(new Product
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Price = reader.GetDouble(reader.GetOrdinal("Price")),
                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                            CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                            Enabled = reader.GetBoolean(reader.GetOrdinal("Enabled")),
                            Category = new Category
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                Name = reader.GetString(reader.GetOrdinal("CategoryName")),
                                Enabled = reader.GetBoolean(reader.GetOrdinal("CategoryEnabled"))
                            }
                        });
                    }
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
            string sql = "UPDATE Products SET Name = @Name, Price = @Price, Quantity = @Quantity, CategoryId = @CategoryId, Enabled = @Enabled WHERE Id = @Id;";
            using (SqlCommand command = new(sql, connection))
            {
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                command.Parameters.AddWithValue("@Enabled", product.Enabled);
                command.Parameters.AddWithValue("@Id", product.Id);
                return await command.ExecuteNonQueryAsync() > 0;
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
                return await command.ExecuteNonQueryAsync() > 0;
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
                return (int)await command.ExecuteScalarAsync() > 0;
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
                return (int)await command.ExecuteScalarAsync() > 0;
            }
        }
    }
}

