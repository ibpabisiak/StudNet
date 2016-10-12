namespace Studnet.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string user_name { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string user_surname { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string user_address_city { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string user_address_street { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string user_address_home_number { get; set; }

        public int user_index { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string user_mail { get; set; }

        public bool user_mail_check { get; set; }

        public int user_study_year { get; set; }

        public int user_semester { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string user_password { get; set; }
    }
}
