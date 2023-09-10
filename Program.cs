using System;
using System.Collections.Generic;
using System.Security.Authentication;

class User
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
}

class BankAccount
{
    public string AccountNumber { get; set; }
    public string AccountHolderName { get; set; }
    public decimal Balance { get; set; }
    public List<string> TransactionHistory { get; set; } = new List<string>();
}

class BankSystem
{
    private static List<User> users = new List<User>();
    private static List<BankAccount> accounts = new List<BankAccount>();
    private static User currentUser;

    static void Main(string[] args)
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("Bank System Menu");
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
            Console.Write("Select an option: ");

            int choice;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        Register();
                        break;
                    case 2:
                        Login();
                        break;
                    case 3:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        }
    }

    static void Register()
    {
        Console.WriteLine("User Registration");
        Console.Write("Enter your name: ");
        string name = Console.ReadLine();
        Console.Write("Enter your email: ");
        string email = Console.ReadLine();

        if (users.Exists(u => u.Email == email))
        {
            Console.WriteLine("Email already registered. Please use a different email.");
            return;
        }

        Console.Write("Enter your password: ");
        string password = Console.ReadLine();

        string encryptedPassword = password;

        User newUser = new User { Name = name, Email = email, Password = encryptedPassword };
        users.Add(newUser);

        Console.WriteLine("Registration successful.");
    }

    static void Login()
    {
        Console.WriteLine("User Login");
        Console.Write("Enter your email: ");
        string email = Console.ReadLine();
        Console.Write("Enter your password: ");
        string password = Console.ReadLine();

        User user = users.Find(u => u.Email == email);

        if (user != null && user.Password == password)
        {
            currentUser = user;
            Console.WriteLine($"Welcome, {user.Name}!");
            ShowAccountMenu();
        }
        else
        {
            Console.WriteLine("Invalid email or password. Please try again.");
        }
    }

    static void ShowAccountMenu()
    {
        while (currentUser != null)
        {
            Console.WriteLine("\nAccount Menu");
            Console.WriteLine("1. Create Bank Account");
            Console.WriteLine("2. Deposit");
            Console.WriteLine("3. Withdraw");
            Console.WriteLine("4. Transfer Money");
            Console.WriteLine("5. Account Information");
            Console.WriteLine("6. Exchange Rate");
            Console.WriteLine("7. Logout");
            Console.Write("Select an option: ");

            int choice;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        CreateBankAccount();
                        break;
                    case 2:
                        Deposit();
                        break;
                    case 3:
                        Withdraw();
                        break;
                    case 4:
                        TransferMoney();
                        break;
                    case 5:
                        AccountInformation();
                        break;
                    case 6:
                        ExchangeRate();
                        break;
                    case 7:
                        currentUser = null;
                        Console.WriteLine("Logged out.");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        }
    }

    static void CreateBankAccount()
    {
        Console.WriteLine("Create Bank Account");
        Console.Write("Enter account holder name: ");
        string accountHolderName = Console.ReadLine();


        string accountNumber = GenerateUniqueAccountNumber();

        Console.Write("Enter initial balance: ");
        decimal initialBalance;
        if (decimal.TryParse(Console.ReadLine(), out initialBalance) && initialBalance >= 0)
        {
            BankAccount newAccount = new BankAccount
            {
                AccountNumber = accountNumber,
                AccountHolderName = accountHolderName,
                Balance = initialBalance
            };

            accounts.Add(newAccount);

            Console.WriteLine($"Account created. Account Number: {accountNumber}");
        }
        else
        {
            Console.WriteLine("Invalid initial balance. Please enter a non-negative number.");
        }
    }

    static string GenerateUniqueAccountNumber()
    {
        Random random = new Random();
        return random.Next(10000000, 999999999).ToString();   
    }

    static void Deposit()
    {
        Console.WriteLine("Deposit");
        Console.Write("Enter account number: ");
        string accountNumber = Console.ReadLine();

        BankAccount account = accounts.Find(a => a.AccountNumber == accountNumber);

        if (account != null)
        {
            Console.Write("Enter deposit amount: ");
            decimal amount;
            if (decimal.TryParse(Console.ReadLine(), out amount) && amount > 0)
            {
                account.Balance += amount;
                account.TransactionHistory.Add($"Deposit: +{amount}");
                Console.WriteLine($"Deposit successful. New balance: {account.Balance}");
            }
            else
            {
                Console.WriteLine("Invalid deposit amount. Please enter a positive number.");
            }
        }
        else
        {
            Console.WriteLine("Account not found.");
        }
    }

    static void Withdraw()
    {
        Console.WriteLine("Withdraw");
        Console.Write("Enter account number: ");
        string accountNumber = Console.ReadLine();

        BankAccount account = accounts.Find(a => a.AccountNumber == accountNumber);

        if (account != null)
        {
            Console.Write("Enter withdrawal amount: ");
            decimal amount;
            if (decimal.TryParse(Console.ReadLine(), out amount) && amount > 0)
            {
                if (account.Balance >= amount)
                {
                    account.Balance -= amount;
                    account.TransactionHistory.Add($"Withdrawal: -{amount}");
                    Console.WriteLine($"Withdrawal successful. New balance: {account.Balance}");
                }
                else
                {
                    Console.WriteLine("Insufficient funds.");
                }
            }
            else
            {
                Console.WriteLine("Invalid withdrawal amount. Please enter a positive number.");
            }
        }
        else
        {
            Console.WriteLine("Account not found.");
        }
    }

    static void TransferMoney()
    {
        Console.WriteLine("Transfer Money");
        Console.Write("Enter your account number: ");
        string senderAccountNumber = Console.ReadLine();

        BankAccount senderAccount = accounts.Find(a => a.AccountNumber == senderAccountNumber);

        if (senderAccount != null)
        {
            Console.Write("Enter recipient's account number: ");
            string recipientAccountNumber = Console.ReadLine();

            BankAccount recipientAccount = accounts.Find(a => a.AccountNumber == recipientAccountNumber);

            if (recipientAccount != null)
            {
                Console.Write("Enter transfer amount: ");
                decimal amount;
                if (decimal.TryParse(Console.ReadLine(), out amount) && amount > 0)
                {
                    if (senderAccount.Balance >= amount)
                    {
                        senderAccount.Balance -= amount;
                        senderAccount.TransactionHistory.Add($"Transfer to {recipientAccountNumber}: -{amount}");

                        recipientAccount.Balance += amount;
                        recipientAccount.TransactionHistory.Add($"Transfer from {senderAccountNumber}: +{amount}");

                        Console.WriteLine($"Transfer successful. New balance: {senderAccount.Balance}");
                    }
                    else
                    {
                        Console.WriteLine("Insufficient funds.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid transfer amount. Please enter a positive number.");
                }
            }
            else
            {
                Console.WriteLine("Recipient's account not found.");
            }
        }
        else
        {
            Console.WriteLine("Your account not found.");
        }
    }

    static void AccountInformation()
    {
        Console.WriteLine("Account Information");
        Console.Write("Enter account number: ");
        string accountNumber = Console.ReadLine();

        BankAccount account = accounts.Find(a => a.AccountNumber == accountNumber);

        if (account != null)
        {
            Console.WriteLine($"Account Number: {account.AccountNumber}");
            Console.WriteLine($"Account Holder: {account.AccountHolderName}");
            Console.WriteLine($"Balance: {account.Balance}");

            Console.WriteLine("Transaction History:");
            foreach (string transaction in account.TransactionHistory)
            {
                Console.WriteLine(transaction);
            }
        }
        else
        {
            Console.WriteLine("Account not found.");
        }
    }

    static void ExchangeRate()
    {
        string fromCurrency = "USD";
        string toCurrency = "EUR";

        decimal exchangeRate = ExchangeRateService.GetExchangeRate(fromCurrency, toCurrency);

        if (exchangeRate >= 0)
        {
            Console.WriteLine($"Exchange rate from {fromCurrency} to {toCurrency}: {exchangeRate:F4}");
        }
        else
        {
            Console.WriteLine("Failed to retrieve exchange rate.");
        }
    }
}
