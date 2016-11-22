namespace Studnet.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Diagnostics;
    using System.Threading;

    public partial class StudnetDatabase : DbContext
    {
        public enum TableType
        {
            Users,
            Forum,
            ForumTopic,
            ForumTopicReply
        }

        public UserManagement UserManagement { get; private set; }
        public ForumManagement ForumManagement { get; private set; }

        public virtual DbSet<forum> forum { get; set; }
        public virtual DbSet<forum_topic> forum_topic { get; set; }
        public virtual DbSet<forum_topic_reply> forum_topic_reply { get; set; }
        public virtual DbSet<users> users { get; set; }

        public ForumManagement ForumManagement1
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public UserManagement UserManagement1
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public forum_topic forum_topic1
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public forum_topic_reply forum_topic_reply1
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public forum forum1
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public users users1
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        private Timer databaseRefreshTimer;

        public StudnetDatabase()
            : base("name=StudnetDatabase")
        {
            UserManagement = new UserManagement(users);
            ForumManagement = new ForumManagement(forum);
            //Set timer for refreshing database which will be launched every 86400 seconds - this is one day
            databaseRefreshTimer = new Timer(RefreshDatabase, null, 1000, 86400000);
        }

        /// <summary>
        /// Method for refreshing database (i.e. users that haven't activate their account for 7 days)
        /// </summary>
        /// <param name="state">State, pass null</param>
        private void RefreshDatabase(object state)
        {
            foreach (var user in users)
            {
                if (!user.user_mail_check && DateTime.Now.Subtract(user.user_date_created).Days >= 7)
                {
                    try
                    {
                        UserManagement.RemoveUser(user);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }
            }
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
                        users.Remove((users)data);
                        break;
                    case TableType.Forum:
                        forum.Remove((forum)data);
                        break;
                    case TableType.ForumTopic:
                        forum_topic.Remove((forum_topic)data);
                        break;
                    case TableType.ForumTopicReply:
                        forum_topic_reply.Remove((forum_topic_reply)data);
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
                        users.Add((users)data);
                        break;
                    case TableType.Forum:
                        forum.Add((forum)data);
                        break;
                    case TableType.ForumTopic:
                        forum_topic.Add((forum_topic)data);
                        break;
                    case TableType.ForumTopicReply:
                        forum_topic_reply.Add((forum_topic_reply)data);
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
            modelBuilder.Entity<forum>()
                .Property(e => e.forum_name)
                .IsUnicode(false);

            modelBuilder.Entity<forum>()
                .HasMany(e => e.forum_topic)
                .WithRequired(e => e.forum)
                .HasForeignKey(e => e.forum_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<forum_topic>()
                .Property(e => e.forum_topic_title)
                .IsUnicode(false);

            modelBuilder.Entity<forum_topic>()
                .HasMany(e => e.forum_topic_reply)
                .WithRequired(e => e.forum_topic)
                .HasForeignKey(e => e.forum_topic_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<forum_topic_reply>()
                .Property(e => e.forum_topic_reply_content)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.user_name)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.user_surname)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.user_address_city)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.user_address_street)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.user_address_home_number)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.user_mail)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.user_password)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .HasMany(e => e.forum_topic)
                .WithRequired(e => e.users)
                .HasForeignKey(e => e.user_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<users>()
                .HasMany(e => e.forum_topic_reply)
                .WithRequired(e => e.users)
                .HasForeignKey(e => e.user_id)
                .WillCascadeOnDelete(false);
        }
    }
}
