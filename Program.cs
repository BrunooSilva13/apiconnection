using System;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using apiconnection;


class Program
{
    static void Main()
    {
        // Defina a string de conexão com o banco de dados
        string connectionString = "server=127.0.0.1;uid=adm-produto;pwd=senha4321;database=api_produto";

        // Crie uma conexão com o banco de dados
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            // Abra a conexão
            connection.Open();

            // Defina a consulta SQL
            string query = "SELECT id, nome, descricao, preco, status_vendas FROM Produto";

            // Crie um comando SQL e atribua a conexão e a consulta
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                // Execute o comando e obtenha um leitor de dados
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    // Verifique se há linhas retornadas
                    while (reader.Read()) // Itera sobre todas as linhas retornadas
                    {
                        int id = reader.GetInt32("id");
                        string nome = reader.GetString("nome");
                        string descricao = reader.GetString("descricao");
                        decimal preco = reader.GetDecimal("preco");
                        bool statusVendas = reader.GetBoolean("status_vendas");

                        // Faça o que quiser com os dados de cada linha
                        Console.WriteLine($"ID: {id}, Nome: {nome}, Descrição: {descricao}, Preço: {preco}, Status de Vendas: {statusVendas}");
                    }
                }
            }
        }

        // Inicie o servidor HTTP em uma tarefa separada após a consulta ao banco de dados
        Task.Run(() => StartHttpServer());

        Console.WriteLine("Pressione qualquer tecla para sair...");
        Console.ReadKey();
    }

    static void StartHttpServer() 
    {
        string url = "http://localhost:8080/";
        // Aqui você precisa criar uma instância da classe HttpServer ou importá-la de uma biblioteca
        HttpServer server = new HttpServer(url);
        server.Start();
        Console.WriteLine($"Servidor HTTP iniciado em {url}...");
    }
}
