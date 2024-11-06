using Generic.OpenAi.DTOs;

namespace Generic.OpenAi.Services.Interfaces
{
    public interface IMessageService
    {
        Task<string> RunAsync(MessageRequest message);
    }
}

