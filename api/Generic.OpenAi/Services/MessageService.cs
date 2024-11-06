using System.Text.Json;
using System.Text;
using Generic.OpenAi.DTOs;
using Generic.OpenAi.Services.Interfaces;

namespace Generic.OpenAi.Services
{
    public class MessageService : IMessageService
    {
        private readonly string? _apiKey;
        private readonly string? _endpoint;
        private readonly string? _context;
        private readonly List<Message> _messages = new();

        public MessageService(IConfiguration configuration)
        {
            _apiKey = configuration["ApiKey"];
            _endpoint = configuration["Endpoint"];
            _context = configuration["Context"];
        }

        public async Task<string> RunAsync(MessageRequest request)
        {
            _messages.Add(new Message { role = "assistant", content = _context });
            _messages.Add(new Message { role = "user", content = request.Message });

            var response = await SendRequestAsync(_messages);

            if (response != null && response.choices.Length > 0)
            {
                var messageResponse = new MessageContent();

                foreach (var choice in response.choices)
                {
                    var jsonString = choice.message.content;

                    jsonString = jsonString.Trim();

                    if (!string.IsNullOrWhiteSpace(jsonString))
                    {
                        try
                        {
                            messageResponse = JsonSerializer.Deserialize<MessageContent>(jsonString);
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"Erro na deserialização: {ex.Message}");
                        }
                    }
                }

                return messageResponse?.clientMessage?.mensagem ?? "Não consegui obter uma resposta.";
            }

            return ("Não consegui obter uma resposta.");
        }

        private async Task<Root?> SendRequestAsync(List<Message> messages)
        {
            var payload = new
            {
                messages = messages,
                temperature = 0.7,
                top_p = 0.95,
                max_tokens = 800,
                stream = false
            };

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);

            var response = await httpClient.PostAsync(_endpoint, new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));

            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var serialization = JsonSerializer.Deserialize<Root>(result);

                return serialization;
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}, {response.ReasonPhrase}");
                return null;
            }
        }
    }
}