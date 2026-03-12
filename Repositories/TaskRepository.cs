using TaskFlowAPI.Data;
using TaskFlowAPI.Models;
using TaskFlowAPI.Helpers;
using TaskFlowAPI.Repositories;

namespace TaskFlowAPI.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;
        public TaskRepository(AppDbContext context)
        {
            _context=context;
        }
        
        public PagedResult<TaskItem> GetAllTasks(int page, int pageSize)
        {
            var query=_context.Tasks.AsQueryable();
            var totalrecords=query.Count();
            var tasks=query.Skip((page-1)*pageSize).Take(pageSize).ToList();
            return new PagedResult<TaskItem>{Page=page,PageSize=pageSize,TotalRecords=totalrecords,Data=tasks};

        }
        public TaskItem AddTask(TaskItem task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return task;
        }
    }
}