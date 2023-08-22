using BankAppDOTNET.Data;
using BankAppDOTNET.Models;


namespace MyBankApplication
{
    class Program
    {
        static void Main(string[] args)
        {
      

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Welcome to MyBank Application!");
                Console.WriteLine("1. User Login");
                Console.WriteLine("2. User Registration");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");

                int choice;
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            UserLogin();
                            break;
                        case 2:
                            UserRegistration();
                            break;
                        case 3:
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
        static void UserLogin()
        {
            using (var dbContext = new BankDbContext())
            {
                Console.Write("Enter your username: ");
                string username = Console.ReadLine();
                Console.Write("Enter your password: ");
                string password = Console.ReadLine();

                var user = dbContext.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
                if (user != null)
                {
                    Console.WriteLine("Login successful!");

                    if (user.Role == UserRole.User)
                    {
                        // Show the user dashboard
                        UserDashboard userDashboard = new UserDashboard(user);
                        userDashboard.ShowDashboard();
                    }
                    else if (user.Role == UserRole.Admin)
                    {
                        // Show the admin dashboard
                        AdminDashboard adminDashboard = new AdminDashboard();
                        adminDashboard.ShowDashboard();
                    }
                    else
                    {
                        Console.WriteLine("Invalid user role. Please contact the administrator.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid username or password. Please try again.");
                }
            }
        }
        private static string GetValidUsernameInput(BankDbContext dbContext)
        {
            while (true)
            {
                Console.Write("Enter a new username (6 to 12 characters, no numbers allowed): ");
                string newUsername = Console.ReadLine();

                // Check if the username length is within the range 6 to 12 characters
                if (newUsername.Length < 6 || newUsername.Length > 12)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Username must be between 6 and 12 characters.");
                    Console.ResetColor();
                    continue;
                }

                // Check if the username contains any numbers
                if (newUsername.Any(char.IsDigit))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Username must not contain any numbers.");
                    Console.ResetColor();
                    continue;
                }

                // Check if the username already exists in the database
                var existingUser = dbContext.Users.FirstOrDefault(u => u.Username == newUsername);
                if (existingUser != null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Username already exists. Please choose a different one.");
                    Console.ResetColor();
                    continue;
                }

                return newUsername;
            }
        }

        private static string GetValidPasswordInput()
        {
            while (true)
            {
                Console.Write("Enter a password (at least 8 characters, containing at least one uppercase letter, one number, and one special character): ");
                string newPassword = Console.ReadLine();

                // Check if the password length is at least 8 characters
                if (newPassword.Length < 8)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Password must be at least 8 characters long.");
                    Console.ResetColor();
                    continue;
                }

                // Check if the password contains at least one uppercase letter, one number, and one special character
                if (!newPassword.Any(char.IsUpper) || !newPassword.Any(char.IsDigit) || !newPassword.Any(c => !char.IsLetterOrDigit(c)))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Password must contain at least one uppercase letter, one number, and one special character.");
                    Console.ResetColor();
                    continue;
                }

                return newPassword;
            }
        }

        static void UserRegistration()
        {
            using (var dbContext = new BankDbContext())
            {
                string newUsername = GetValidUsernameInput(dbContext);

                // Check if the username already exists
                var existingUser = dbContext.Users.FirstOrDefault(u => u.Username == newUsername);
                if (existingUser != null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Username already exists. Please choose a different one.");
                    Console.ResetColor();
                    return;
                }

                string newPassword = GetValidPasswordInput();

                // Validate name (should not be empty or whitespace)
                string name;
                while (true)
                {
                    Console.Write("Enter your name: ");
                    name = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        break;
                    }

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Name cannot be empty or whitespace.");
                    Console.ResetColor();
                }

                // Create the new user
                var newUser = new User
                {
                    Username = newUsername,
                    Password = newPassword,
                    Name = name,
                    Balance = 12000 // Set initial balance as required
                };

                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();

                Console.WriteLine("Registration successful! You can now log in with your new credentials.");
            }
        }

    }

    public class UserDashboard
    {
        private readonly User currentUser;

        public UserDashboard(User user)
        {
            currentUser = user;
        }

        public void ShowDashboard()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine($"Welcome, {currentUser.Name}!");
                Console.WriteLine("1. Check Balance");
                Console.WriteLine("2. Withdraw Money");
                Console.WriteLine("3. Transfer Money to Another Account");
                Console.WriteLine("4. Get a Loan");
                Console.WriteLine("5. View Transaction Logs");
                Console.WriteLine("6. Logout");
                Console.Write("Enter your choice: ");

                int choice;
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            CheckBalance();
                            break;
                        case 2:
                            WithdrawMoney();
                            break;
                        case 3:
                            TransferMoney();
                            break;
                        case 4:
                            GetLoan();
                            break;
                        case 5:
                            ViewTransactionLogs();
                            break;
                        case 6:
                            exit = true;
                            Console.WriteLine("Logged out successfully.");
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void CheckBalance()
        {
            Console.WriteLine($"Your current balance is: {currentUser.Balance:C}");
        }

        private decimal GetValidAmountInput()
        {
            decimal amount;
            while (true)
            {
                Console.Write("Enter the amount: ");
                if (decimal.TryParse(Console.ReadLine(), out amount) && amount > 0)
                {
                    return amount;
                }

                // Set text color to red for error message
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid amount. Please enter a valid positive number.");
                // Reset text color to default
                Console.ResetColor();
            }
        }


        public void WithdrawMoney()
        {
            decimal amountToWithdraw = GetValidAmountInput();
            if (amountToWithdraw <= currentUser.Balance)
            {
                currentUser.Balance -= amountToWithdraw;
                UpdateUserBalance();
                Console.WriteLine($"Successfully withdrawn {amountToWithdraw:C}. Your current balance is: {currentUser.Balance:C}");
                LogTransaction(amountToWithdraw, TransactionType.Withdrawal);
            }
            else
            {
                Console.WriteLine("Insufficient balance.");
            }
        }

        public void TransferMoney()
        {
            Console.Write("Enter the recipient's username: ");
            string recipientUsername = Console.ReadLine();

            using (var dbContext = new BankDbContext())
            {
                var recipientUser = dbContext.Users.FirstOrDefault(u => u.Username == recipientUsername);
                if (recipientUser == null)
                {
                    Console.WriteLine("Recipient user not found. Please check the username and try again.");
                    return;
                }

                decimal amountToTransfer = GetValidAmountInput();
                if (amountToTransfer <= currentUser.Balance)
                {
                    currentUser.Balance -= amountToTransfer;
                    recipientUser.Balance += amountToTransfer;
                    UpdateUserBalance();
                    Console.WriteLine($"Successfully transferred {amountToTransfer:C} to {recipientUser.Name}.");
                    Console.WriteLine($"Your current balance is: {currentUser.Balance:C}");
                    LogTransaction(amountToTransfer, TransactionType.Transfer);
                }
                else
                {
                    Console.WriteLine("Insufficient balance.");
                }
            }
        }

        private void LogTransaction(decimal amount, TransactionType type)
        {
            using (var dbContext = new BankDbContext())
            {
                var transactionLog = new TransactionLog
                {
                    UserId = currentUser.Id,
                    Amount = amount,
                    Type = type,
                    Timestamp = DateTime.Now
                };
                dbContext.TransactionLogs.Add(transactionLog);
                dbContext.SaveChanges();
            }
        }

        private void GetLoan()
        {
            Console.Write("Enter the loan amount: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal loanAmount) && loanAmount > 0)
            {
                if (currentUser.GetLoan(loanAmount))
                {
                    UpdateUserBalance();
                    Console.WriteLine($"Loan of {loanAmount:C} granted. Your current balance is: {currentUser.Balance:C}");
                }
                else
                {
                    Console.WriteLine("Loan not granted. Check eligibility or request a lower amount.");
                }
            }
            else
            {
                Console.WriteLine("Invalid loan amount. Please enter a valid positive number.");
            }
        }

        private void UpdateUserBalance()
        {
            using (var dbContext = new BankDbContext())
            {
                var userToUpdate = dbContext.Users.Find(currentUser.Id);
                userToUpdate.Balance = currentUser.Balance;
                dbContext.SaveChanges();
            }
        }

        
    public void ViewTransactionLogs()
    {
        using (var dbContext = new BankDbContext())
        {
            var transactionLogs = dbContext.TransactionLogs
                .Where(log => log.UserId == currentUser.Id)
                .OrderByDescending(log => log.Timestamp)
                .ToList();

            if (transactionLogs.Count == 0)
            {
                Console.WriteLine("No transaction logs found for your account.");
            }
            else
            {
                Console.WriteLine("Transaction Logs:");
                foreach (var log in transactionLogs)
                {
                    string transactionType = log.Type == TransactionType.Withdrawal ? "Withdrawal" : "Transfer";
                    Console.WriteLine($"{log.Timestamp} - {transactionType} - Amount: {log.Amount:C}");
                }
            }
        }
    }
    }

    public class AdminDashboard
    {
        public void ShowDashboard()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Welcome, Admin!");
                Console.WriteLine("1. View User Accounts");
                Console.WriteLine("2. View Transaction Logs");
                Console.WriteLine("3. Logout");
                Console.Write("Enter your choice: ");

                int choice;
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            ViewUserAccounts();
                            break;
                        case 2:
                            ViewTransactionLogs();
                            break;
                        case 3:
                            exit = true;
                            Console.WriteLine("Logged out successfully.");
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void ViewUserAccounts()
        {
            using (var dbContext = new BankDbContext())
            {
                var users = dbContext.Users.ToList();
                if (users.Count > 0)
                {
                    Console.WriteLine("List of User Accounts:");
                    foreach (var user in users)
                    {
                        Console.WriteLine($"User ID: {user.Id}, Username: {user.Username}, Name: {user.Name}, Balance: {user.Balance:C}");
                    }
                }
                else
                {
                    Console.WriteLine("No user accounts found.");
                }
            }
        }

        private void ViewTransactionLogs()
        {
            using (var dbContext = new BankDbContext())
            {
                var transactions = dbContext.TransactionLogs.ToList();
                if (transactions.Count > 0)
                {
                    Console.WriteLine("Transaction Logs:");
                    foreach (var transaction in transactions)
                    {
                        Console.WriteLine($"Transaction ID: {transaction.Id}, User ID: {transaction.UserId}, Type: {transaction.Type}, Amount: {transaction.Amount:C}, Timestamp: {transaction.Timestamp}");
                    }
                }
                else
                {
                    Console.WriteLine("No transaction logs found.");
                }
            }
        }
    }

}




