using RESTServer.Models;
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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Message { get; set; }
        public string Placement { get; set; }
        public DateTime Date { get; set; }
        public int SeverityId { get; set; }
        public Severity Severity { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Created { get; set; }
        public int CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedById { get; set; }
        public User? ModifiedBy { get; set; }
        public string Reason { get; set; }
        public bool Archived { get; set; }
    }
}
