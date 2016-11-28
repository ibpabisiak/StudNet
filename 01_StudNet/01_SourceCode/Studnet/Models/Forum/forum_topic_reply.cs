namespace Studnet.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class forum_topic_reply
    {
        public int Id { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string forum_topic_reply_content { get; set; }

        [Column(TypeName = "date")]
        public DateTime forum_topic_reply_date_created { get; set; }

        public int forum_topic_id { get; set; }

        public int user_id { get; set; }

        public virtual forum_topic forum_topic { get; set; }

        public virtual users users { get; set; }
    }
}
