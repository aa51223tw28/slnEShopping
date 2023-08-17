using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class UserChatVM
    {
        public int SellerId { get; set; }

        public string StoreName { get; set; }

        public string ChatroomId { get; set; }

        public string UserName { get; set; }
    }
}