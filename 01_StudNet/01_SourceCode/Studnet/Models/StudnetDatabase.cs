namespace Studnet.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class StudnetDatabase : DbContext
    {
        public StudnetDatabase()
            : base("name=StudnetDatabase")
        {
        }

        public virtual DbSet<User> users { get; set; }

        /// <summary>
        /// Method which adds user to database
        /// </summary>
        /// <param name="user">Validated user to add</param>
        public void AddUser(User user)
        {
            users.Add(user);
            SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(e => e.user_name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.user_surname)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.user_address_city)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.user_address_street)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.user_address_home_number)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.user_mail)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.user_password)
                .IsUnicode(false);
        }
    }
}
