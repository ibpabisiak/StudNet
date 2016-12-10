namespace Studnet.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("event")]
    public partial class _event
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string event_title { get; set; }

        [Required]
        [StringLength(1024)]
        public string event_description { get; set; }

        public DateTime event_start { get; set; }

        public DateTime event_end { get; set; }
    }
}
