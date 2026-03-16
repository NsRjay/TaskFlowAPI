using Microsoft.AspNetCore.Mvc;
using TaskFlowAPI.Data;
using TaskFlowAPI.Models;
using TaskFlowAPI.Repositories;
using TaskFlowAPI.DTOs;
using TaskFlowAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace TaskFlowAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService=taskService;
        }
        [HttpGet]
        [ResponseCache(Duration=30)]
        public IActionResult GetTasks(int page=1 ,int pageSize=5)
        {
            var tasks= _taskService.GetAllTasks(page,pageSize);
            return Ok(tasks);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        
        public IActionResult CreateTask(TaskCreateDTO dto)
        {
            var task=_taskService.CreateTask(dto);

            return Ok(task);

        }
       
    }
}