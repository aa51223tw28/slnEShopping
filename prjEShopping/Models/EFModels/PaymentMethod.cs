namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PaymentMethod
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PaymentMethodId { get; set; }

        [StringLength(50)]
        public string PaymentMethodName { get; set; }
    }
}
