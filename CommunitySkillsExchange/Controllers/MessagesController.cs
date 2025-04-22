using CommunitySkillsExchange.Data;
using CommunitySkillsExchange.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunitySkillsExchange.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Messages
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var conversations = await _context.Conversations
                .Where(c => c.User1Id == currentUser.Id || c.User2Id == currentUser.Id)
                .Include(c => c.User1)
                .Include(c => c.User2)
                .OrderByDescending(c => c.LastMessageAt)
                .ToListAsync();

            return View(conversations);
        }

        // GET: Messages/Conversation/5
        public async Task<IActionResult> Conversation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var conversation = await _context.Conversations
                .Include(c => c.User1)
                .Include(c => c.User2)
                .Include(c => c.Messages)
                .ThenInclude(m => m.Sender)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (conversation == null)
            {
                return NotFound();
            }

            // Security check - users can only view conversations they're part of
            if (conversation.User1Id != currentUser.Id && conversation.User2Id != currentUser.Id)
            {
                return Forbid();
            }

            // Mark unread messages as read
            var unreadMessages = conversation.Messages
                .Where(m => m.SenderId != currentUser.Id && !m.IsRead)
                .ToList();

            foreach (var message in unreadMessages)
            {
                message.IsRead = true;
            }

            if (unreadMessages.Any())
            {
                await _context.SaveChangesAsync();
            }

            // Get other user
            ViewData["OtherUser"] = conversation.User1Id == currentUser.Id ? conversation.User2 : conversation.User1;

            return View(conversation);
        }

        // POST: Messages/SendMessage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(int conversationId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return BadRequest("Message cannot be empty");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var conversation = await _context.Conversations
                .FirstOrDefaultAsync(c => c.Id == conversationId);

            if (conversation == null)
            {
                return NotFound();
            }

            // Security check - users can only send messages in conversations they're part of
            if (conversation.User1Id != currentUser.Id && conversation.User2Id != currentUser.Id)
            {
                return Forbid();
            }

            var message = new Message
            {
                ConversationId = conversationId,
                SenderId = currentUser.Id,
                Content = content,
                SentAt = DateTime.Now,
                IsRead = false
            };

            conversation.LastMessageAt = DateTime.Now;

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Conversation), new { id = conversationId });
        }

        // GET: Messages/StartConversation/userId
        public async Task<IActionResult> StartConversation(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var otherUser = await _userManager.FindByIdAsync(userId);
            if (otherUser == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);

            // Check if conversation already exists
            var existingConversation = await _context.Conversations
                .FirstOrDefaultAsync(c =>
                    (c.User1Id == currentUser.Id && c.User2Id == userId) ||
                    (c.User1Id == userId && c.User2Id == currentUser.Id));

            if (existingConversation != null)
            {
                return RedirectToAction(nameof(Conversation), new { id = existingConversation.Id });
            }

            // Create new conversation
            var conversation = new Conversation
            {
                User1Id = currentUser.Id,
                User2Id = userId,
                CreatedAt = DateTime.Now,
                LastMessageAt = DateTime.Now
            };

            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Conversation), new { id = conversation.Id });
        }
    }
}
