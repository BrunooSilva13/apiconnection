using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("teste")]
public class MeuController : ControllerBase
{
    private readonly ILogger<MeuController> _logger;

    public MeuController(ILogger<MeuController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult Ola()
    {

        return  Ok("BEm vindo a minha aplicação");

    }

}
