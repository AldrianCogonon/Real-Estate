using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly ChatService _chatService;

    public ChatController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost]
    public IActionResult Chat(UserMessage message)
    {
        var reply = _chatService.GenerateReply(message.Message);

        return Ok(new { reply });
    }
}