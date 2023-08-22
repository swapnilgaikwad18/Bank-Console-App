using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAppDOTNET.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public UserRole Role { get; set; }
        public TransactionType Type { get; set; }
        public List<TransactionLog> Transactions { get; set; } = new List<TransactionLog>();
        // Add a collection to represent the accounts associated with the user
        public ICollection<Account> Accounts { get; set; } = new List<Account>();

        public bool GetLoan(decimal amount)
        {
            // Add logic to determine if the user is eligible for a loan and update the balance accordingly
            if (amount > 0 && amount <= Balance * 0.5m) // Allowing a loan up to 50% of the current balance
            {
                Balance += amount;
                return true;
            }
            return false;
        }
    }
    public enum UserRole
    {
       User,
       Admin
    }

}
