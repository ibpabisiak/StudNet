using Studnet.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Studnet
{
    public class ForumManagement
    {
        private DbSet<forum> forum;

        public ForumManagement(DbSet<forum> forumTable)
        {
            forum = forumTable;
        }

        /// <summary>
        /// Method which checks if thread with given name exists in given forum
        /// </summary>
        /// <param name="threadForum">Forum</param>
        /// <param name="thread_name">Thread name to check</param>
        /// <returns>True if thread with given name exists</returns>
        public bool CheckIfThreadExists(forum threadForum, string thread_name)
        {
            return threadForum.forum_topic.Any(m => m.forum_topic_title == thread_name);
        }

        /// <summary>
        /// Method which adds thread to forum
        /// </summary>
        /// <param name="threadForum">Forum to add thread to</param>
        /// <param name="thread">Thread to add</param>
        /// <param name="threadPost">First required post in thread</param>
        /// <param name="user_mail">Creator email adress</param>
        public void AddThread(forum threadForum, forum_topic thread, forum_topic_reply threadPost, string user_mail)
        {
            try
            {
                users creator = AppData.Instance().StudnetDatabase.users.Where(m => m.user_mail == user_mail).Single();
                thread.forum_topic_date_created = DateTime.Now;
                thread.users = creator;
                thread.forum = threadForum;
                AppData.Instance().StudnetDatabase.AddRecordToTable(StudnetDatabase.TableType.ForumTopic, thread);
                threadPost.forum_topic = thread;
                threadPost.forum_topic_reply_date_created = DateTime.Now;
                threadPost.users = creator;
                AppData.Instance().StudnetDatabase.AddRecordToTable(StudnetDatabase.TableType.ForumTopicReply, threadPost);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method which adds reply to fiven thread
        /// </summary>
        /// <param name="thread">Thread to add reply to</param>
        /// <param name="threadPost">Reply</param>
        /// <param name="user_mail">Creator email adress</param>
        public void AddReply(forum_topic thread, forum_topic_reply threadPost, string user_mail)
        {
            try
            {
                threadPost.forum_topic = thread;
                threadPost.forum_topic_reply_date_created = DateTime.Now;
                threadPost.users = AppData.Instance().StudnetDatabase.users.Where(m => m.user_mail == user_mail).Single();
                AppData.Instance().StudnetDatabase.AddRecordToTable(StudnetDatabase.TableType.ForumTopicReply, threadPost);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}