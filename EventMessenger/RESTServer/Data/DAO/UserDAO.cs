using Cassandra;
using RESTServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTServer.Data.DAO
{
    public class UserDAO: CassandraDAO
    {
        public List<User> GetUsers()
        {
            string strCQL = "SELECT * FROM user";
            Session localSession = GetSession();
            RowSet results = localSession.Execute(strCQL);

            List<User> users = new List<User>();

            foreach (var row in results.GetRows())
                users.Add(mapUser(row));

            return users;
        }

        private User mapUser(Row row)
        {
            if (row == null) return null;

            return new User()
            {
                AmountOfLogins = row.GetValue<int>("amountoflogins"),
                Active = row.GetValue<bool>("active"),
                BirthId = row.GetValue<string>("birthid"),
                Email = row.GetValue<string>("email"),
                FirstName = row.GetValue<string>("firstname"),
                HasAdmin = row.GetValue<bool>("hasadmin"),
                Id = row.GetValue<int>("id"),
                IsDeleted = row.GetValue<bool>("isdeleted"),
                Modified = row.GetValue<DateTime?>("modified"),
                ModifiedById = row.GetValue<int?>("modifiedbyid"),
                Password = row.GetValue<string>("password"),
                SirName = row.GetValue<string>("sirname")
            };
        }

        public User GetUser(int id)
        {
            Session localSession = GetSession();
            var statement = new SimpleStatement("SELECT * FROM user WHERE id = ?", id);
            RowSet result = localSession.Execute(statement);

            return mapUser(result.GetRows().FirstOrDefault());
        }

        public bool UserExists(string birthId)
        {
            Session localSession = GetSession();
            var statement = new SimpleStatement("SELECT * FROM user WHERE birthid = ?", birthId);
            RowSet result = localSession.Execute(statement);

            return mapUser(result.GetRows().FirstOrDefault()) != null;
        }

        public int CreateUser(User user)
        {
            Session localSession = GetSession();
            RowSet results = null;
            if (user.Id == 0) {
                string strCQL = "SELECT MAX(id) FROM user";
                results = localSession.Execute(strCQL);
                var id = results.GetRows().FirstOrDefault().FirstOrDefault();

                if (id == null)
                    id = 1;

                user.Id = (int)id;
            }

            var statement = new SimpleStatement("INSERT INTO user (amountoflogins, active, birthid, email, firstname, hasadmin, id, isdeleted, modified, modifiedbyid, password, sirname)" +
                "VALUES (?,?,?,?,?,?,?,?,?,?,?,?)", user.AmountOfLogins, user.Active, user.BirthId, user.Email, user.FirstName, user.HasAdmin, user.Id, user.IsDeleted, user.Modified, user.ModifiedById, user.Password, user.SirName);
            localSession.Execute(statement);

            return user.Id;
        }

        public void UpdateUser(User user)
        {
            Session localSession = GetSession();
            var statement = new SimpleStatement("UPDATE user SET firstname =?, sirname =?, password =?, birthid =?, email =?, hasadmin =? " +
                                                "WHERE id =?", user.FirstName, user.SirName, user.Password, user.BirthId, user.Email, user.HasAdmin, user.Id);
            localSession.Execute(statement);
        }

        public void AuthenticateUser(int amountOfLogins, int userId)
        {
            Session localSession = GetSession();
            var statement = new SimpleStatement("UPDATE user SET amountoflogins =? WHERE id =?", amountOfLogins + 1, userId);
            localSession.Execute(statement);
        }

        public bool AuthenticateAdmin()
        {
            //Get birthid and password from currently logged in user
            //Session localSession = GetSession();
            //var statement = new SimpleStatement("SELECT * FROM user WHERE birthid=? AND password=? AND hasadmin='True'", birthId, pass);
            //RowSet result = localSession.Execute(statement);

            //return mapUser(result.GetRows().FirstOrDefault()) != null;
            return true;
        }
    }
}
