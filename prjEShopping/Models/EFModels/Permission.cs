namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Permission
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Permission()
        {
            Admins = new HashSet<Admin>();
        }

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Admin> Admins { get; set; }
    }
}
