using Studnet.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Studnet
{
    public class MessageManagement
    {
        private DbSet<message> message;

        public MessageManagement(DbSet<message> messageTable)
        {
            message = messageTable;
        }

        public void SendMessage(message messageToSend)
        {
            AppData.Instance().StudnetDatabase.AddRecordToTable(StudnetDatabase.TableType.Message, messageToSend);
        }
    }
}