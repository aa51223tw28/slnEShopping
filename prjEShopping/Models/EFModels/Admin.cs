namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Admin
    {
        public int AdminId { get; set; }

        [StringLength(50)]
        public string AdminNumber { get; set; }

        public int? PermissionsId { get; set; }

        [StringLength(50)]
        public string AdminAccount { get; set; }

        [StringLength(50)]
        public string AdminPassword { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(50)]
        public string AdminName { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        public DateTime? DateOfHire { get; set; }

        [StringLength(50)]
        public string JobStatus { get; set; }
    }
}
