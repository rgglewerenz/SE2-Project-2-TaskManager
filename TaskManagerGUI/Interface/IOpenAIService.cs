namespace TaskManagerGUI.Interface
{
    public interface IOpenAIService
    {
        Task<string> GetDescriptionForTaskByTitle(string taskTitle);
    }
}