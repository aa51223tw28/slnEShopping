namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AccessRight
    {
        public int AccessRightId { get; set; }

        [StringLength(50)]
        public string AccessRightName { get; set; }
    }
}
