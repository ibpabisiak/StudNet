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
            try
            {
                if (users.FirstOrDefault(m => m.user_mail == user.user_mail) != null)
                {
                    throw new Exception("User already exists in database");
                }
                else
                {
                    users.Add(user);
                    SaveChanges();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public User AuthorizeUser(string mail, string password)
        {
            var user = users.FirstOrDefault(m => m.user_mail == mail);

            if(user == null)
            {
                throw new Exception("Invalid email");
            }
            else if(user.user_password != password)
            {
                throw new Exception("Invalid password");
            }
            else if(!user.user_mail_check)
            {
                throw new Exception("User not activated");
            }

            return user;
        }

        public bool CheckIfUserExists(User user)
        {
            bool ifUserExists = false;

            if (users.FirstOrDefault(m => m.user_mail == user.user_mail) != null)
            {
                ifUserExists = true;
            }
            return ifUserExists;
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
