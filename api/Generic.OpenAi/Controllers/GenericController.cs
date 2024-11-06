using Microsoft.AspNetCore.Mvc;
using Generic.OpenAi.DTOs;
using Generic.OpenAi.Services.Interfaces;

namespace Generic.OpenAi.Controllers;

[ApiController]
[Route("[controller]")]
public class GenericController : ControllerBase
{
    private readonly IMessageService _service;

    public GenericController(IMessageService service)
    {
        _service = service;
    }

    [HttpPost(Name = "chatbot")]
    public async Task<IActionResult> Post([FromBody] MessageRequest message)
    {
        try
        {
            var result = await _service.RunAsync(message);

            var responseMessage = result; 

            return Ok(new { Message = responseMessage });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }
}

