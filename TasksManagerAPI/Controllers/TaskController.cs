using DatabaseBAL;
using DatabaseInterop.Models;
using DTO;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
namespace TasksManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private TaskBAL _taskBAL = new TaskBAL();


        [HttpGet("GetTasks/{UserID:int}")]
        public List<TaskTransferModal> GetTasks(int UserID)
        {
            return _taskBAL.GetTasksTransferModals(UserID);
        }


        [HttpPost("CreateTasks/{UserID:int}")]
        public bool CreateTask(int UserID, string title, string description, DateTime FirstOccurance, DatabaseEnums.RecurrentTypes RecurringType, string? RecurringDays ) {
            try{
                _taskBAL.AddTask(new TaskTransferModal()
                {
                    Title = title,
                    Description = description,
                    recurrenceOptions = new TaskRecurrenceTransferModal()
                    {
                        RecurringType = RecurringType,
                        RecurringDays = RecurringDays,
                        FirstOccurrance = FirstOccurance
                    }
                },
                UserID);
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        [HttpPost("UpdateTask/{UserID:int}")]
        public bool UpdateTask(int UserID, TaskTransferModal task)
        {
            try
            {
                _taskBAL.UpdateTask(task, UserID);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
