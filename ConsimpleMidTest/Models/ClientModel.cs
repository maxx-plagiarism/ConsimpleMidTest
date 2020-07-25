using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsimpleMidTest.Models
{
    public class ClientModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
