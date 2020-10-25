using RESTServer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RESTServer.Models
{
    public class Reminder
    {
        [Key]
        public string Id { get; set; }
        [ForeignKey("BadEvent")]
        public string EventId { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
    }
}
