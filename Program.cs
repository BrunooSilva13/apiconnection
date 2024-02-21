using System;
using MySql.Data.MySqlClient;

class Program
{
    static void Main()
    {
        // Defina a string de conexão com o banco de dados
        //string connectionString = "server=localhost;database=api_produto;uid=adm-produto;pwd=senha4321;";

        string connectionString = "server=127.0.0.1;uid=adm-produto;pwd=senha4321;database=api_produto";
        // Crie uma conexão com o banco de dados
         using (MySqlConnection connection = new MySqlConnection(connectionString))
         {
            // Abra a conexão
            connection.Open();

            // Defina a consulta SQL
            string query = "SELECT * FROM Produto";

            // Crie um comando SQL e atribua a conexão e a consulta
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                
                // Execute o comando e obtenha um leitor de dados
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    // Verifique se há linhas retornadas
                    if (reader.Read())
                    {
                        //
                        // Obtenha o valor da coluna e armazene em uma variável
                        int idProduto = reader.GetInt16(0);
                        string product = reader.GetString(1);
                        float price = reader.GetFloat(3);
                        bool isBuy = reader.GetBoolean(4);
                        // Faça algo com o valor obtido
                        System.Console.WriteLine("ID do Produto: " + idProduto);
                        Console.WriteLine("Nome do produto: " + product);
                        System.Console.WriteLine("valor do Produto: " + price);
                        System.Console.WriteLine("O produto foi vendido: " + isBuy);

                    }
                    else
                    {
                        Console.WriteLine("Nenhuma linha retornada.");
                    }
                }
            }
        }
    }    
}