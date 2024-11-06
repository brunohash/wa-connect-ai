namespace Generic.OpenAi.DTOs
{
    public class MessageContent
    {
        public ClientMessage clientMessage { get; set; }
        public OperatorInstruction operatorInstruction { get; set; }
    }

    public class ClientMessage
    {
        public string? mensagem { get; set; }
    }

    public class OperatorInstruction
    {
        public string? mensagem { get; set; }
        public string? tag { get; set; }
    }
}

