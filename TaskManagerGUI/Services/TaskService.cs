using DTO;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using TaskManagerGUI.Interface;

namespace TaskManagerGUI.Services
{
    public class TaskService : BaseWebApi, ITaskService
    {
        bool pingApi = false;
        public TaskService(HttpClient httpClient, IConfiguration configuration) : base(httpClient, configuration)
        {
            BaseURL = configuration.GetValue<string>("BaseApiUrl");
            pingApi = configuration.GetValue<bool>("PingApi");
        }

        public async Task<bool> CreateTask(int UserID, TaskTransferModal modal)
        {
            return pingApi ? await PostInfo<TaskTransferModal, bool>(BaseURL + $"Task/CreateTask/{UserID}", modal) : true;
        }

        public async Task<List<TaskTransferModal>> GetTasks(int UserID)
        {
            return pingApi ? await GetInfoFromJson<List<TaskTransferModal>>(BaseURL + $"Task/GetTasks/{UserID}") : new List<TaskTransferModal>()
            {
                new TaskTransferModal()
                {
                    Description = "Sample Description",
                    Title = "Sample Title"
                }

            };
        }

        public async Task<bool> UpdateTask(int UserID, TaskTransferModal modal)
        {
            return pingApi ? await PostInfo<TaskTransferModal, bool>(BaseURL + $"Task/UpdateTask/{UserID}", modal) : true;
        }


        public async Task<bool> DeleteTask(int TaskID)
        {
            if(!pingApi)
            {
                return true;
            }

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, BaseURL + $"Task/DeleteTask/{TaskID}");
            var response = await _http.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode){
                return Convert.ToBoolean(await response.Content.ReadAsStringAsync());
            }
            return false;
        }

    }
}
