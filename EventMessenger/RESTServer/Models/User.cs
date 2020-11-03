using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RESTServer.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string BirthId { get; set; } // Hashed value
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string SirName { get; set; }
        public int AmountOfLogins { get; set; }
        public bool HasAdmin { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedById { get; set; }
        public User? ModifiedBy { get; set; }
        public bool Active { get; set; } // not in DB
    }
}
