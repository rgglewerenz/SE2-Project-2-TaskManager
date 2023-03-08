using Microsoft.AspNetCore.Mvc;
using System.Data.Entity.Infrastructure;
using System.Reflection.Metadata.Ecma335;

namespace TasksManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpenAIController : ControllerBase
    {
        private readonly OpenAI_API.OpenAIAPI _openAIApi;

        public OpenAIController(IConfiguration _config)
        {
            _openAIApi = new OpenAI_API.OpenAIAPI(_config.GetSection("OpenAICredentials").GetSection("APIKey").Value);
        }

        [HttpGet("CompleteTaskDescriptionByTitle")]
        public async Task<string> CompleteTaskDescriptionByTitle(string title) {

            var request = new OpenAI_API.Completions.CompletionRequest()
            {
                Temperature = 0.2,
                Model = OpenAI_API.Models.Model.DavinciText,
                Prompt = $"Based on the title of a task create a paragraph (less than 500 characters) inference about the task's purpose. Task title: {title}.",
                MaxTokens = 200,
                
            };
            var result = await _openAIApi.Completions.CreateCompletionAsync(request);
            return result.ToString();
        }

    }
}
