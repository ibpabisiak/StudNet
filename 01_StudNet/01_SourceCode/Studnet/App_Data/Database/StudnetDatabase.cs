namespace Studnet.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Diagnostics;

    public partial class StudnetDatabase : DbContext
    {
        public enum TableType
        {
            Users
        }

        public UserManagement UserManagement { get; private set; }
        public virtual DbSet<User> Users { get; set; }

        public StudnetDatabase()
            : base("name=StudnetDatabase")
        {
            UserManagement = new UserManagement(Users);
        }

        /// <summary>
        /// Method which removes given data from given table
        /// </summary>
        /// <param name="table">Table to remove data from</param>
        /// <param name="data">Data to remove</param>
        public void RemoveRecordFromTable(TableType table, object data)
        {
            try
            {
                switch (table)
                {
                    case TableType.Users:
                        Users.Remove((User)data);
                        break;
                }
                SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method which adds given data to selected table in database
        /// </summary>
        /// <param name="table">Table to add data to</param>
        /// <param name="data">Data to add</param>
        public void AddRecordToTable(TableType table, object data)
        {
            try
            {
                switch (table)
                {
                    case TableType.Users:
                        Users.Add((User)data);
                        break;
                }
                SaveChanges();
            }
            catch(Exception ex)
            {
                try
                {
                    RemoveRecordFromTable(table, data);
                }
                catch(Exception e)
                {
                    Debug.WriteLine(e);
                }
                finally
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Method which creates database model
        /// </summary>
        /// <param name="modelBuilder">Model builder</param>
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
