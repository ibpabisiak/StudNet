namespace Studnet.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public users()
        {
            forum_topic = new HashSet<forum_topic>();
            forum_topic_reply = new HashSet<forum_topic_reply>();
            message = new HashSet<message>();
            message1 = new HashSet<message>();
        }

        public int Id { get; set; }

        [Required]
        public string user_name { get; set; }

        [Required]
        public string user_surname { get; set; }

        [Required]
        public string user_address_city { get; set; }

        [Required]
        public string user_address_street { get; set; }

        [Required]
        public string user_address_home_number { get; set; }

        public int user_index { get; set; }

        [Required]
        public string user_mail { get; set; }

        public bool user_mail_check { get; set; }

        public int user_study_year { get; set; }

        public int user_semester { get; set; }

        [Required]
        public string user_password { get; set; }

        [Column(TypeName = "date")]
        public DateTime user_date_created { get; set; }

        public int user_rank_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<forum_topic> forum_topic { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<forum_topic_reply> forum_topic_reply { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<message> message { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<message> message1 { get; set; }

        public virtual rank rank { get; set; }

        public string GetFullName()
        {
            return user_name + " " + user_surname;
        }
    }
}
