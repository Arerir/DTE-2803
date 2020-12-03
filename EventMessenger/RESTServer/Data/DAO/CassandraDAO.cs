using Cassandra;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTServer.Data.DAO
{
    public class CassandraDAO
    {
        private Cluster cluster;
        private Session session;
        private String NODE = Startup.Configuration.GetConnectionString("CassandraConnection");
        //Use these if the database has integrated security added strings the same way as the connectionpoint
        //private String USER = Startup.Configuration.GetConnectionString("CassandraUser");
        //private String PASS = Startup.Configuration.GetConnectionString("CassandraPwd");

        public CassandraDAO()
        {
            connect();
        }

        private void connect()
        {
            //connect to Cassandra cluster
            cluster = Cluster.Builder().AddContactPoint(NODE).Build();
            var i = cluster.Configuration.SocketOptions.SetConnectTimeoutMillis(50000);
            session = (Session)cluster.Connect("restserver");
        }

        protected Session GetSession()
        {
            //return the session if it's already built
            if(session == null)
            {
                connect();
            }
            return session;
        }
    }
}
