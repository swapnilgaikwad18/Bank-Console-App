using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAppDOTNET.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }

        // Add a foreign key to represent the association with the user
        public int UserId { get; set; }
        public User User { get; set; }


    }
}
