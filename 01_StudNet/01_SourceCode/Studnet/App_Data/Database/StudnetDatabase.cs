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
            ForumTopicReply,
            Rank,
            ForumTopicCategory,
            Event,
            Message,
            Group
        }

        //public UserManagement UserManagement { get; private set; }
        //public ForumManagement ForumManagement { get; private set; }
        public UserManagement UserManagement { get; private set; }
        public ForumManagement ForumManagement { get; private set; }
        public MessageManagement MessageManagement { get; private set; }
        public GroupManagement GroupManagement { get; private set; }

        public virtual DbSet<_event> _event { get; set; }
        public virtual DbSet<forum> forum { get; set; }
        public virtual DbSet<forum_topic> forum_topic { get; set; }
        public virtual DbSet<forum_topic_category> forum_topic_category { get; set; }
        public virtual DbSet<forum_topic_reply> forum_topic_reply { get; set; }
        public virtual DbSet<group> group { get; set; }
        public virtual DbSet<message> message { get; set; }
        public virtual DbSet<rank> rank { get; set; }
        public virtual DbSet<users> users { get; set; }


        //pola nizej musza byc ze wzgledu na UML.. uzywamy Entity, ale visual
        //go nie uznaje do UML wiec ... dlatego nie chcemy UML z Visual Studio
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
            GroupManagement = new GroupManagement(group);
            UserManagement = new UserManagement(users);
            ForumManagement = new ForumManagement(forum);
            MessageManagement = new MessageManagement(message);
            //Set timer for refreshing database which will be launched every 86400 seconds - this is one day
            databaseRefreshTimer = new Timer(RefreshDatabase, null, 1000, 86400000);
        }

        /// <summary>
        /// Method for refreshing database (i.e. users that haven't activate their account for 7 days)
        /// </summary>
        /// <param name="state">State, pass null</param>
        private void RefreshDatabase(object state)
        {
            try
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
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
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
                    case TableType.Rank:
                        rank.Remove((rank)data);
                        break;
                    case TableType.ForumTopicCategory:
                        forum_topic_category.Remove((forum_topic_category)data);
                        break;
                    case TableType.Event:
                        _event.Remove((_event)data);
                        break;
                    case TableType.Message:
                        message.Remove((message)data);
                        break;
                    case TableType.Group:
                        group.Remove((group)data);
                        break;
                }
                SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTableEntry(TableType table, object data)
        {
            object currentEntry = null;
            try
            {
                switch (table)
                {
                    case TableType.Users:
                        currentEntry = users.Where(m => m.Id == ((users)data).Id).Single();
                        users.Remove((users)currentEntry);
                        users.Add((users)data); break;
                    case TableType.Forum:
                        currentEntry = forum.Where(m => m.Id == ((forum)data).Id).Single();
                        forum.Remove((forum)currentEntry);
                        forum.Add((forum)data); break;
                    case TableType.ForumTopic:
                        currentEntry = forum_topic.Where(m => m.Id == ((forum_topic)data).Id).Single();
                        forum_topic.Remove((forum_topic)currentEntry);
                        forum_topic.Add((forum_topic)data); break;
                    case TableType.ForumTopicReply:
                        currentEntry = forum_topic_reply.Where(m => m.Id == ((forum_topic_reply)data).Id).Single();
                        forum_topic_reply.Remove((forum_topic_reply)currentEntry);
                        forum_topic_reply.Add((forum_topic_reply)data); break;
                    case TableType.Rank:
                        currentEntry = rank.Where(m => m.Id == ((rank)data).Id).Single();
                        rank.Remove((rank)currentEntry);
                        rank.Add((rank)data); break;
                    case TableType.ForumTopicCategory:
                        currentEntry = forum_topic_category.Where(m => m.Id == ((forum_topic_category)data).Id).Single();
                        forum_topic_category.Remove((forum_topic_category)currentEntry);
                        forum_topic_category.Add((forum_topic_category)data);
                        break;
                    case TableType.Event:
                        currentEntry = _event.Where(m => m.Id == ((_event)data).Id).Single();
                        _event.Remove((_event)currentEntry);
                        _event.Add((_event)data);
                        break;
                    case TableType.Message:
                        currentEntry = message.Where(m => m.Id == ((message)data).Id).Single();
                        message.Remove((message)currentEntry);
                        message.Add((message)data);
                        break;
                    case TableType.Group:
                        currentEntry = group.Where(m => m.Id == ((group)data).Id).Single();
                        group.Remove((group)currentEntry);
                        group.Add((group)data);
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
                    case TableType.Rank:
                        rank.Add((rank)data);
                        break;
                    case TableType.ForumTopicCategory:
                        forum_topic_category.Add((forum_topic_category)data);
                        break;
                    case TableType.Event:
                        _event.Add((_event)data);
                        break;
                    case TableType.Message:
                        message.Add((message)data);
                        break;
                    case TableType.Group:
                        group.Add((group)data);
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
            modelBuilder.Entity<_event>()
                .Property(e => e.event_title)
                .IsUnicode(false);

            modelBuilder.Entity<_event>()
                .Property(e => e.event_description)
                .IsUnicode(false);

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

            modelBuilder.Entity<forum_topic_category>()
                .Property(e => e.forum_topic_category_name)
                .IsUnicode(false);

            modelBuilder.Entity<forum_topic_reply>()
                .Property(e => e.forum_topic_reply_content)
                .IsUnicode(false);

            modelBuilder.Entity<group>()
                .Property(e => e.group_name)
                .IsUnicode(false);

            modelBuilder.Entity<group>()
                .HasMany(e => e.forum_topic)
                .WithOptional(e => e.group)
                .HasForeignKey(e => e.forum_topic_group_id);

            modelBuilder.Entity<group>()
                .HasMany(e => e.users)
                .WithOptional(e => e.group)
                .HasForeignKey(e => e.user_group_id);

            modelBuilder.Entity<message>()
                .Property(e => e.message_text)
                .IsUnicode(false);

            modelBuilder.Entity<rank>()
                .Property(e => e.rank_name)
                .IsUnicode(false);

            modelBuilder.Entity<rank>()
                .HasMany(e => e.users)
                .WithRequired(e => e.rank)
                .HasForeignKey(e => e.user_rank_id)
                .WillCascadeOnDelete(false);

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

            modelBuilder.Entity<users>()
                .HasMany(e => e.message)
                .WithRequired(e => e.users)
                .HasForeignKey(e => e.message_user_from_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<users>()
                .HasMany(e => e.message1)
                .WithRequired(e => e.users1)
                .HasForeignKey(e => e.message_user_to_id)
                .WillCascadeOnDelete(false);
        }
    }
}
