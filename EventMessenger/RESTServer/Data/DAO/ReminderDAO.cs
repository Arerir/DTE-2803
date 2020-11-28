using Cassandra;
using RESTServer.Entities;
using RESTServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTServer.Data.DAO
{
    public class ReminderDAO : CassandraDAO
    {
        public List<Reminder> GetReminders()
        {
            var strCQL = $@"SELECT * FROM reminder";
            Session localSession = GetSession();
            var results = localSession.Execute(strCQL);

            List<Reminder> reminders = new List<Reminder>();

            foreach (var row in results.GetRows())
                reminders.Add(mapEvent(row));

            return reminders;
        }

        private Reminder mapEvent(Row row)
        {
            if (row == null) return null;

            return new Reminder()
            {
                Created = row.GetValue<DateTime>("created"),
                CreatedById = row.GetValue<int>("createdbyid"),
                Date = row.GetValue<DateTime>("date"),
                EventId = row.GetValue<int>("eventid"),
                Id = row.GetValue<int>("id"),
                IsDeleted = row.GetValue<bool>("isdeleted"),
                Message = row.GetValue<string>("message"),
                Modified = row.GetValue<DateTime?>("modified"),
                ModifiedById = row.GetValue<int?>("modifiedbyid")
            };
        }

        public Reminder GetReminder(int id)
        {
            Session localSession = GetSession();
            var statement =new SimpleStatement("SELECT * FROM reminder WHERE id =?", id);
            var result = localSession.Execute(statement);

            return mapEvent(result.GetRows().FirstOrDefault());
        }

        public int CreateReminder(Reminder reminder, int currentUserId = 1)
        {
            Session localSession = GetSession();
            RowSet results = null;
            if (reminder.Id == 0)
            {
                string strCQL = "SELECT MAX(id) FROM reminder";
                results = localSession.Execute(strCQL);
                var id = results.GetRows().FirstOrDefault().FirstOrDefault();

                if (id == null)
                    id = 1;

                reminder.Id = (int)id;
            }

            var statement = new SimpleStatement("INSERT INTO reminder (created, createdbyid, date, eventid, id, isdeleted, message, modified, modifiedbyid)" +
                "VALUES (?,?,?,?,?,?,?,?,?)", DateTime.Now, currentUserId, reminder.Date, reminder.EventId, reminder.Id, false, reminder.Message, null, null);
            localSession.Execute(statement);

            return reminder.Id;
        }

        public void UpdateReminder(Reminder reminder)
        {
            Session localSession = GetSession();
            var statement = new SimpleStatement("UPDATE reminder SET date =?, eventid =?, message =?, modified =?, modifiedbyid =?, isdeleted =?" +
                                                "WHERE id =?", reminder.Date, reminder.EventId, reminder.Message, reminder.Modified, reminder.ModifiedById, reminder.IsDeleted);
            localSession.Execute(statement);
        }
    }
}
