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
        private TaskBAL _taskBAL;

        public TaskController(IConfiguration _config)
        {
            _taskBAL = new TaskBAL(_config);
        }

        #region Get

        [HttpGet("GetTasks/{UserID:int}")]
        public List<TaskTransferModal> GetTasks(int UserID)
        {
            return _taskBAL.GetTasksTransferModals(UserID);
        }

        #endregion Get

        #region Post

        [HttpPost("CreateTask/{UserID:int}")]
        public bool CreateTask(int UserID, TaskTransferModal taskTransfer) {
            try{
                return _taskBAL.AddTask(taskTransfer, UserID);
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        [HttpPost("UpdateTask/{UserID:int}")]
        public bool UpdateTask(int UserID, TaskTransferModal task)
        {
            try
            {
                return _taskBAL.UpdateTask(task, UserID);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion Post

        #region Delete

        [HttpDelete("DeleteTask/{TaskID:int}")]
        public bool DeleteTask(int TaskID)
        {
            try
            {
                return _taskBAL.DeleteTask(TaskID);
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        #endregion Delete
    }
}
