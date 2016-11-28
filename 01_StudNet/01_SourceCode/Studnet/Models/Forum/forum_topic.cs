namespace Studnet.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class forum_topic
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public forum_topic()
        {
            forum_topic_reply = new HashSet<forum_topic_reply>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string forum_topic_title { get; set; }

        public int user_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime forum_topic_date_created { get; set; }

        public int forum_id { get; set; }

        public int forum_topic_category_id { get; set; }

        public virtual forum forum { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<forum_topic_reply> forum_topic_reply { get; set; }

        public virtual users users { get; set; }
    }
}
