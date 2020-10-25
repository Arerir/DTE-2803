using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTServer.Data.DTO
{
    public class BadEventDTO
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Placement { get; set; }
        public DateTime Date { get; set; }
        public int SeverityId { get; set; }
        public int StatusId { get; set; }
    }
}
