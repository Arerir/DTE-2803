using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTServer.Models
{
    public class Status
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
    }
}
