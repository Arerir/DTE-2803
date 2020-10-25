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
        public int Id { get; set; }
        public int EventId { get; set; }
        public BadEvent Event { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Created { get; set; }
        public int CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedById { get; set; }
        public User? ModifiedBy { get; set; }
    }
}
