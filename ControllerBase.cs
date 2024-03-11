using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

[ApiController]
[Route("api/products")]
public class MyController : ControllerBase
{
    private readonly string _connectionString;

    public MyController(IConfiguration configuration)
    {
        _connectionString = "server=127.0.0.1;uid=adm-product;pwd=senha4321;";
        using (MySqlConnection mysqlConnection = new MySqlConnection(_connectionString))
        {
            mysqlConnection.Open();
            using (MySqlCommand cmd = mysqlConnection.CreateCommand())
            {
                cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS api_product";
                cmd.ExecuteNonQuery();

                cmd.CommandText = $"USE api_product";
                cmd.ExecuteNonQuery();

                // products
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS products (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        Name VARCHAR(255),
                        Description VARCHAR(255),
                        Price FLOAT,
                        Status BOOLEAN
                    );";
                cmd.ExecuteNonQuery();
            }
        }
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            using (var connection = new MySqlConnection(_connectionString + "database=api_product;"))
            {
                connection.Open();

                string query = "SELECT * FROM products";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var result = new List<object>();
                        while (reader.Read())
                        {
                            var item = new
                            {
                                Name = reader["Name"],
                                Description = reader["Description"],
                                Price = reader["Price"],
                                Status = reader["Status"]
                            };
                            result.Add(item);
                        }

                        return Ok(result);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public IActionResult Post(Product product)
    {
        try
        {
          //condicionais para verificar se todos os campos que o front tá enviando existem 
          
            
                if (product == null || string.IsNullOrEmpty(product.Name) || string.IsNullOrEmpty(product.Description) || product.Price <= 0 || product.Status == null)
                {
                    return BadRequest("all fields are mandatory.");
                }

          
                using (var connection = new MySqlConnection(_connectionString + "database=api_product;"))
                {
                    connection.Open();
                    string query = "INSERT INTO products (Name, Description, Price, Status) VALUES (@name, @description, @price, @status)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", product.Name);
                        command.Parameters.AddWithValue("@description", product.Description);
                        command.Parameters.AddWithValue("@price", product.Price);
                        command.Parameters.AddWithValue("@status", product.Status);
                        command.ExecuteNonQuery();
                    }
                }   
            return Ok("product created successfully!");
            
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error creating product: {ex.Message}");
        }
    }
    [HttpPut("{id}")]
    public IActionResult Put(int id, Product product)
    {
        try
        {
            if (product == null || string.IsNullOrEmpty(product.Name) || string.IsNullOrEmpty(product.Description) || product.Price <= 0 || product.Status == null)
                {
                    return BadRequest("all fields are mandatory.");
                }
            
            using (var connection = new MySqlConnection(_connectionString + "database=api_product;"))
            {
                connection.Open();
                string query = "UPDATE products SET Name = @name, Description = @description, Price = @price, Status = @status WHERE Id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@name", product.Name);
                    command.Parameters.AddWithValue("@description", product.Description);
                    command.Parameters.AddWithValue("@price", product.Price);
                    command.Parameters.AddWithValue("@status", product.Status);
                    command.ExecuteNonQuery();
                }
            }
            return Ok("product updated successfully!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error updating product: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            using (var connection = new MySqlConnection(_connectionString + "database=api_product;"))
            {
                connection.Open();
                string query = "DELETE FROM products WHERE Id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
            return Ok("product successfully deleted!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error deleting product: {ex.Message}");
        }
    }

}
