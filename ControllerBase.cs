using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

[ApiController]
[Route("api/produtos")]
public class MyController : ControllerBase
{
    private readonly string _connectionString;

    public MyController(IConfiguration configuration)
    {
        _connectionString = "server=127.0.0.1;uid=adm-produto;pwd=senha4321;";
        using (MySqlConnection mysqlConnection = new MySqlConnection(_connectionString))
        {
            mysqlConnection.Open();
            using (MySqlCommand cmd = mysqlConnection.CreateCommand())
            {
                cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS api_produto";
                cmd.ExecuteNonQuery();

                cmd.CommandText = $"USE api_produto";
                cmd.ExecuteNonQuery();

                // Produtos
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS produtos (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        Nome VARCHAR(255),
                        Descricao VARCHAR(255),
                        Preco FLOAT,
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
            using (var connection = new MySqlConnection(_connectionString + "database=api_produto;"))
            {
                connection.Open();

                string query = "SELECT * FROM produtos";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var result = new List<object>();
                        while (reader.Read())
                        {
                            var item = new
                            {
                                Nome = reader["Nome"],
                                Descricao = reader["Descricao"],
                                Preco = reader["Preco"],
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
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }

    [HttpPost]
    public IActionResult Post(Produto produto)
    {
        try
        {
            using (var connection = new MySqlConnection(_connectionString + "database=api_produto;"))
            {
                connection.Open();
                string query = "INSERT INTO produtos (Nome, Descricao, Preco, Status) VALUES (@nome, @descricao, @preco, @status)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nome", produto.Nome);
                    command.Parameters.AddWithValue("@descricao", produto.Descricao);
                    command.Parameters.AddWithValue("@preco", produto.Preco);
                    command.Parameters.AddWithValue("@status", produto.Status);
                    command.ExecuteNonQuery();
                }
            }
            return Ok("Produto criado com sucesso!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao criar produto: {ex.Message}");
        }
    }
}
