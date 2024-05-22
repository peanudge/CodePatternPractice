using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace WebApi;

[Route("api/item")]
[ApiController]
public class ItemController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Message = "Hello World!" });
    }

    [HttpGet("fail")]
    public IActionResult GetFail()
    {
        return BadRequest(new { Message = "Something went wrong!" });
    }

    [HttpGet("{id}")]
    public IActionResult NotFound([FromRoute] long id)
    {
        return NotFound(new { Message = $"Item with id {id} not found!" });
    }

    [HttpGet("error")]
    public IActionResult ServierInternalError()
    {
        return Problem("ERROR", statusCode: StatusCodes.Status500InternalServerError);
    }
}
