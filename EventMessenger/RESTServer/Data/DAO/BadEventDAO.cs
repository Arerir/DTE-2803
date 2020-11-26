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
        public List<BadEvent> GetBadEvents()
        {
            var strCQL = "SELECT * FROM badevent";
            Session localSession = GetSession();
            var results = localSession.Execute(strCQL);

            List<BadEvent> users = new List<BadEvent>();

            foreach (var row in results.GetRows())
            {
                users.Add(mapEvent(row));
            }

            return users;
        }

        private BadEvent mapEvent(Row row)
        {
            if (row == null) return null;

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
                ModifiedById = row.GetValue<int>("modifiedbyid"),
                Placement = row.GetValue<string>("placement"),
                Reason = row.GetValue<string>("reason"),
                SeverityId = row.GetValue<int>("severityid"),
                StatusId = row.GetValue<int>("statusid")
            };
        }

        public BadEvent GetBadEvent(int id)
        {
            Session localSession = GetSession();
            var statement = new SimpleStatement("SELECT * FROM badevent WHERE id =?", id);
            var result = localSession.Execute(statement);

            return mapEvent(result.GetRows().FirstOrDefault());
        }

        public int CreateBadEvent(BadEvent badEvent)
        {
            var currentuserId = 1;
            Session localSession = GetSession();
            RowSet results = null;
            if (badEvent.Id == 0)
            {
                string strCQL = "SELECT MAX(id) FROM badevent";
                results = localSession.Execute(strCQL);
                var id = results.GetRows().FirstOrDefault().FirstOrDefault();

                if (id == null)
                {
                    id = 1;
                }
                badEvent.Id = (int)id;
            }

            var statement = new SimpleStatement("INSERT INTO badevent (created, createdbyid, date, id, isdeleted, message, modified, modifiedbyid, placement, reason, severityid, statusid)" +
                "VALUES (?,?,?,?,?,?,?,?,?,?,?,?)", DateTime.Now, currentuserId, badEvent.Date, badEvent.Id, false, badEvent.Message, null, null, badEvent.Placement, badEvent.Reason, badEvent.SeverityId, badEvent.StatusId);
            localSession.Execute(statement);

            return badEvent.Id;
        }
    }
}
