using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TasksV2Controller : ControllerBase
{
    [HttpGet]
    public IActionResult GetTasksV2()
    {
        return Ok("This is Tasks API version 2");
    }
}