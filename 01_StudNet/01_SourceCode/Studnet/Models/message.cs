namespace Studnet.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("message")]
    public partial class message
    {
        public int Id { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string message_text { get; set; }

        public int message_user_from_id { get; set; }

        public int message_user_to_id { get; set; }

        public DateTime message_date_created { get; set; }

        public bool message_is_read { get; set; }

        public virtual users users { get; set; }

        public virtual users users1 { get; set; }
    }
}
