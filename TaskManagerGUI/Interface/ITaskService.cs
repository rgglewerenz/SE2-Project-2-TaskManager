using DTO;

namespace TaskManagerGUI.Interface
{
    public interface ITaskService
    {
        Task<bool> CreateTask(int UserID, TaskTransferModal modal);
        Task<bool> DeleteTask(int TaskID);
        Task<List<TaskTransferModal>> GetTasks(int UserID);
        Task<bool> UpdateTask(int UserID, TaskTransferModal modal);
    }
}
