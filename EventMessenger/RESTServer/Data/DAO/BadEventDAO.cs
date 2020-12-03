using Cassandra;
using RESTServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTServer.Data.DAO
{
    public class BadEventDAO : CassandraDAO
    {
        public List<BadEvent> GetBadEvents(bool archived)
        {
            //Set up select query and execute
            var strCQL = "SELECT * FROM badevent";
            Session localSession = GetSession();
            var results = localSession.Execute(strCQL);

            List<BadEvent> badEvents = new List<BadEvent>();

            foreach (var row in results.GetRows())
                badEvents.Add(mapEvent(row));

            //return mapped objects
            return badEvents.Where(x => x.Archived == archived).ToList();
        }

        private BadEvent mapEvent(Row row)
        {
            if (row == null) return null;
            //connect the rows in database to the rows in the database
            return new BadEvent()
            {
                Archived = row.GetValue<bool>("archived"),
                Created = row.GetValue<DateTime>("created"),
                CreatedById = row.GetValue<int>("createdbyid"),
                Date = row.GetValue<DateTime>("date"),
                Id = row.GetValue<int>("id"),
                IsDeleted = row.GetValue<bool>("isdeleted"),
                Message = row.GetValue<string>("message"),
                Modified = row.GetValue<DateTime?>("modified"),
                ModifiedById = row.GetValue<int?>("modifiedbyid"),
                Placement = row.GetValue<string>("placement"),
                Reason = row.GetValue<string>("reason"),
                SeverityId = row.GetValue<int>("severityid"),
                StatusId = row.GetValue<int>("statusid")
            };
        }

        public BadEvent GetBadEvent(int id)
        {
            //get session and create statement before executing
            Session localSession = GetSession();
            var statement = new SimpleStatement("SELECT * FROM badevent WHERE id =?", id);
            var result = localSession.Execute(statement);
            //return mapped object
            return mapEvent(result.GetRows().FirstOrDefault());
        }

        public int CreateBadEvent(BadEvent badEvent, int currentUserId = 1)
        {
            Session localSession = GetSession();
            RowSet results = null;
            //If setting to not collide with data from previous solution
            if (badEvent.Id == 0)
            {
                //get new Id field for the object
                string strCQL = "SELECT MAX(id) FROM badevent";
                results = localSession.Execute(strCQL);
                var id = results.GetRows().FirstOrDefault().FirstOrDefault();

                //Set to 1 if table is empty
                if (id == null)
                    id = 1;

                badEvent.Id = (int)id;
            }

            //insert object into database
            var statement = new SimpleStatement("INSERT INTO badevent (id, archived, created, createdbyid, date, isdeleted, message, modified, modifiedbyid, placement, reason, severityid, statusid)" +
                "VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?)", badEvent.Id, badEvent.Archived, DateTime.Now, currentUserId, badEvent.Date, false, badEvent.Message, null, null, badEvent.Placement, badEvent.Reason, badEvent.SeverityId, badEvent.StatusId);
            localSession.Execute(statement);

            return badEvent.Id;
        }

        public void UpdateBadEvent(BadEvent badEvent)
        {
            //update in database
            Session localSession = GetSession();
            var statement = new SimpleStatement("UPDATE badevent SET date =?, message =?, placement =?, reason =?, severityid =?, statusid =?, modified =?, modifiedbyid =?, isdeleted =?" +
                                                "WHERE id =?", badEvent.Date, badEvent.Message, badEvent.Placement, badEvent.Reason, badEvent.SeverityId, badEvent.StatusId, badEvent.Modified, badEvent.ModifiedById, badEvent.IsDeleted);
            localSession.Execute(statement);
        }

        public void ArchiveBadEvent(int badEventId)
        {
            //archive selected event
            Session localSession = GetSession();
            var statement = new SimpleStatement("UPDATE badevent SET archived = 'true' WHERE id =?", badEventId);
            localSession.Execute(statement);
        }
    }
}
