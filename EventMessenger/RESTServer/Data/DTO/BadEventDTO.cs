using RESTServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RESTServer.Data.DTO
{
    public class BadEventDTO
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Reason { get; set; }
        public string Placement { get; set; }
        public DateTime Date { get; set; }
        public int SeverityId { get; set; }
        public int StatusId { get; set; }

        public static Expression<Func<BadEvent, BadEventDTO>> Selector()
        {
            return x => new BadEventDTO
            {
                Id = x.Id,
                Message = x.Message,
                Reason = x.Reason,
                Placement = x.Placement,
                Date = x.Date,
                SeverityId = x.SeverityId,
                StatusId = x.StatusId
            };
        }
    }
}
