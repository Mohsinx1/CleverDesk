using CleverDesk.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CleverDesk.Models.Data;

[Authorize]
public class AIAssistantController : Controller
{
    private readonly AppDbContext _context;
    private readonly GeminiService _geminiService;

    private int LoggedInUserId => int.Parse(User.FindFirstValue("UserId"));

    public AIAssistantController(AppDbContext context, GeminiService geminiService)
    {
        _context = context;
        _geminiService = geminiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var messages = await _context.ChatMessages
            .Where(m => m.UserId == LoggedInUserId)
            .OrderBy(m => m.Timestamp)
            .ToListAsync();

        return View(messages);
    }

    [HttpPost]
    public async Task<IActionResult> SendMessageAjax([FromBody] string message)
    {
        var userMessage = new ChatMessage
        {
            UserId = LoggedInUserId,
            Role = "User",
            Message = message,
            Timestamp = DateTime.Now
        };

        _context.ChatMessages.Add(userMessage);
        await _context.SaveChangesAsync();

        var aiResponse = await _geminiService.GetGeminiResponseAsync(message);

        var aiReply = new ChatMessage
        {
            UserId = LoggedInUserId,
            Role = "AI",
            Message = aiResponse,
            Timestamp = DateTime.Now
        };

        _context.ChatMessages.Add(aiReply);
        await _context.SaveChangesAsync();

        return Json(new
        {
            user = userMessage.Message,
            ai = aiReply.Message
        });
    }


}
