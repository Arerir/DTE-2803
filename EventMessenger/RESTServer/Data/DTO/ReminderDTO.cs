using RESTServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RESTServer.Data.DTO
{
    public class ReminderDTO
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime ReminderDate { get; set; }
        public string Message { get; set; }
        public string SendtFrom { get; set; }

        public static Expression<Func<Reminder, ReminderDTO>> Selector()
        {
            return x => new ReminderDTO
            {
                Id = x.Id,
                EventId = x.EventId,
                EventDate = x.Date,
                Message = x.Message,
                ReminderDate = x.Created
            };
        }
    }
}
