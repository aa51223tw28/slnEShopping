using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendMessage(string userMessage)
        {
            string botResponse = GetBotResponse(userMessage);

            List<ChatMessageVM> chatHistory = new List<ChatMessageVM>
            {
                new ChatMessageVM { Sender = "User", Message = userMessage },
                new ChatMessageVM { Sender = "Bot", Message = botResponse }
            };

            ViewBag.ChatHistory = chatHistory;

            return View("SendMessage");
        }

    private string GetBotResponse(string userMessage)
    {
        // TODO: Implement your bot logic here, you can use external APIs or libraries for natural language processing
        // For this example, let's just echo the user message
        return "Bot: " + userMessage;
    }
    }
}