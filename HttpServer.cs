using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace apiconnection {
public class HttpServer
{
    private readonly string _url;
    private readonly HttpListener _listener;

    public HttpServer(string url)
    {
        _url = url;
        _listener = new HttpListener();
        _listener.Prefixes.Add(_url);
    }

    public void Start()
    {
        _listener.Start();
        Console.WriteLine($"Servidor HTTP iniciado em {_url}...");
        Task.Run(() => Listen());
    }

    public void Stop()
    {
        _listener.Stop();
        _listener.Close();
    }

    private async Task Listen()
    {
        while (_listener.IsListening)
        {
            HttpListenerContext context = await _listener.GetContextAsync();
            HandleRequest(context);
        }
    }

    private void HandleRequest(HttpListenerContext context)
{
    HttpListenerRequest request = context.Request;
    HttpListenerResponse response = context.Response;

    // Configura o tipo de conteúdo da resposta
    response.ContentType = "application/json";

    // Verifica se a solicitação é um GET para /produto
    if (request.HttpMethod == "GET" && request.Url.AbsolutePath == "/produto")
    {
        // Aqui você pode adicionar a lógica para buscar informações sobre produtos do seu banco de dados
        // Por enquanto, vamos apenas enviar uma resposta fixa como exemplo
        string responseString = "[{\"id\": 1, \"nome\": \"Produto 1\", \"descricao\": \"Descrição do Produto 1\", \"preco\": 10.00, \"status_vendas\": true}]";

        // Escreve a resposta de volta ao cliente
        WriteResponse(response, responseString);
    }
    else
    {
        // Se a solicitação não for para /produto ou não for um GET, retorna um erro 404
        response.StatusCode = 404;
    }

    // Fecha o fluxo de saída da resposta
    response.Close();
}


    // private void HandleRequest(HttpListenerContext context)
    // {
    //     HttpListenerRequest request = context.Request;
    //     HttpListenerResponse response = context.Response;

    //     response.ContentType = "application/json";

    //     if (request.HttpMethod == "GET" && request.Url.AbsolutePath == "/produto")
    //     {
    //         string responseString = "[{\"id\": 1, \"nome\": \"Produto 1\", \"descricao\": \"Descrição do Produto 1\", \"preco\": 10.00, \"status_vendas\": true}]";
    //         WriteResponse(response, responseString);
    //     }
    //     else
    //     {
    //         response.StatusCode = 404;
    //     }

    //     response.Close();
    // }

    private void WriteResponse(HttpListenerResponse response, string responseString)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(responseString);
        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer, 0, buffer.Length);
    }   
    }

}

























