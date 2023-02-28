using DTO;
using TaskManagerGUI.Interface;

namespace TaskManagerGUI.Services
{
    public class TaskService : BaseWebApi, ITaskService
    {
        public TaskService(HttpClient httpClient, IConfiguration configuration) : base(httpClient, configuration)
        {
            BaseURL = configuration.GetValue<string>("BaseApiUrl");
        }

        public async Task<bool> CreateTask(int UserID, TaskTransferModal modal)
        {
            return await PostInfo<TaskTransferModal, bool>(BaseURL + $"Task/CreateTask/{UserID}", modal);
        }

    }
}
