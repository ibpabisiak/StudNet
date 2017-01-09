using Studnet.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Studnet
{
    public class GroupManagement
    {
        private DbSet<group> group;

        public GroupManagement(DbSet<group> groupTable)
        {
            group = groupTable;
        }

        public void RemoveUserFromGroup(users userToRemove, group groupToRemoveFrom)
        {
            try
            {
                groupToRemoveFrom.users.Remove(userToRemove);
                AppData.Instance().StudnetDatabase.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddGroup(group newGroup)
        {
            try
            {
                if(newGroup.group_name == null || newGroup.group_name.Length < 1)
                {
                    throw new Exception("Nazwa grupy musi zawierać co najmniej 1 znak");
                }
                AppData.Instance().StudnetDatabase.AddRecordToTable(StudnetDatabase.TableType.Group, newGroup);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddUserToGroup(users userToAdd, group groupToAddTo)
        {
            try
            {
                if (groupToAddTo.users.Contains(userToAdd))
                {
                    throw new Exception("User is already in group");
                }
                groupToAddTo.users.Add(userToAdd);
                AppData.Instance().StudnetDatabase.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}