using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class UserForLogin : IDto
    {
        public string EMail { get; set; }
        public string Password { get; set; }
    }
}
