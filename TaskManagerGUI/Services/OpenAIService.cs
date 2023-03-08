using System.Web;
using TaskManagerGUI.Interface;

namespace TaskManagerGUI.Services
{
    public class OpenAIService: BaseWebApi, IOpenAIService
    {
        bool pingApi = false;

        public OpenAIService(HttpClient httpClient, IConfiguration configuration) : base(httpClient, configuration)
        {
            BaseURL = configuration.GetValue<string>("BaseApiUrl");
            pingApi = configuration.GetValue<bool>("PingApi");
        }


        public async Task<string> GetDescriptionForTaskByTitle(string taskTitle)
        {
            if (!pingApi)
                return "";

            var result = await GetInfoString(BaseURL + $"OpenAI/CompleteTaskDescriptionByTitle?title={HttpUtility.UrlEncode(taskTitle)}");
			return result.TrimStart();
        }

    }
}
