using RESTServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RESTServer.Data.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string SirName { get; set; }

        public static Expression<Func<User, UserDTO>> Selector()
        {
            return x => new UserDTO
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                SirName = x.SirName
            };
        }
    }
}
