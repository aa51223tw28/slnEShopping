using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 如需如何設定應用程式的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkID=316888
            app.MapSignalR();
        }
    }
}