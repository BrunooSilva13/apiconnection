using apiConnection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class MyController : ControllerBase
{
    private readonly string _connectionString;

    public MyController(IConfiguration configuration)
    {
        _connectionString = "server=127.0.0.1;uid=adm-produto;pwd=senha4321;database=api_produto";
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Execute sua consulta SQL aqui
                // Por exemplo:
                string query = "SELECT * FROM Produto";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        // Processar os resultados da consulta
                        // Por exemplo, criar uma lista de objetos ou retornar os dados diretamente
                        // Exemplo:
                        var result = new List<object>();
                        while (reader.Read())
                        {
                            var item = new
                            {
                                Produto = reader["nome"],
                                Descrição = reader["descricao"],
                                Preço = reader["preco"],
                                Status = reader["status_vendas"]
                                // Adicione mais propriedades conforme necessário
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
            // Trate qualquer exceção aqui
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }

    [HttpPost]
    public IActionResult Post(Produto produto)
    {
        try
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Produto (nome, descricao, preco, status_vendas) VALUES (@nome, @descricao, @preco, @status_vendas)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nome", produto.Nome);
                    command.Parameters.AddWithValue("@descricao", produto.Descricao);
                    command.Parameters.AddWithValue("@preco", produto.Preco);
                    command.Parameters.AddWithValue("@status_vendas", produto.Status);
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
