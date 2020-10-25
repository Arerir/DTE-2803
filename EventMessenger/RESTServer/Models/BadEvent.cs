using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RESTServer.Entities
{
    public class BadEvent
    {
        [Key]
        public string Id { get; set; }
        public string Message { get; set; }
        public string Placement { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey("Severity")]
        public int SeverityId { get; set; }
        [ForeignKey("Status")]
        public int StatusId { get; set; }
    }
}
