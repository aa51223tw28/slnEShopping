namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Rating
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rating()
        {
            RatingReplaies = new HashSet<RatingReplay>();
        }

        public int RatingId { get; set; }

        public int? ProductId { get; set; }

        public int? UserId { get; set; }

        public int? StarRating { get; set; }

        [StringLength(4000)]
        public string RatingText { get; set; }

        public DateTime? PostTime { get; set; }

        [StringLength(50)]
        public string RatingStatus { get; set; }

        public virtual Product Product { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RatingReplay> RatingReplaies { get; set; }
    }
}
