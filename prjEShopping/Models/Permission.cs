//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace prjEShopping.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Permission
    {
        public int PermissionId { get; set; }
        public int PermissionLevel { get; set; }
        public string PermissionName { get; set; }
        public string PlatformPermission { get; set; }
        public string UserPermission { get; set; }
        public string SellerPermission { get; set; }
        public string ProductPermission { get; set; }
        public string SupportPermission { get; set; }
    }
}
