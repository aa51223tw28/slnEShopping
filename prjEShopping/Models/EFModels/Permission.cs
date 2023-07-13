namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Permission
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PermissionId { get; set; }

        public int PermissionLevel { get; set; }

        [StringLength(50)]
        public string PermissionName { get; set; }

        [StringLength(50)]
        public string PlatformPermission { get; set; }

        [StringLength(50)]
        public string UserPermission { get; set; }

        [StringLength(50)]
        public string SellerPermission { get; set; }

        [StringLength(50)]
        public string ProductPermission { get; set; }

        [StringLength(50)]
        public string SupportPermission { get; set; }
    }
}
