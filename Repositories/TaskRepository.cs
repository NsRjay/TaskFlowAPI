using TaskFlowAPI.Data;
using TaskFlowAPI.Models;
using TaskFlowAPI.Helpers;
using TaskFlowAPI.Repositories;
using Microsoft.Identity.Client;

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
        
        public TaskItem UpdateTask(TaskItem task)
        {
            var existingtask=_context.Tasks.FirstOrDefault(t=>t.Id==task.Id);
            if (existingtask==null)
            {
                throw new Exception ("Task not found");
            }
            //update task
            existingtask.Title=task.Title;
            existingtask.Description=task.Description;
            existingtask.IsCompleted=task.IsCompleted;
            _context.SaveChanges();
            //  if task object is fully valid then you can use _context.tasks.Update(task);_context.SaveChanges();
            
            return existingtask;
        }

        public TaskItem DeleteTask(TaskItem task)
        {   
            var existingtask=_context.Tasks.FirstOrDefault(t=>t.Id==task.Id);
            if(existingtask==null)
            {
                throw new Exception("Task not found");
            }
            _context.Tasks.Remove(existingtask);
            _context.SaveChanges();
            return existingtask;
        }
        public TaskItem GetTaskById(int id)
        {
            return _context.Tasks.FirstOrDefault(t=>t.Id==id);
        }
    }
}