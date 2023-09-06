using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"{new string('-', 35)}Banking Application{new string('-', 36)}");
            Console.WriteLine();
            Accounts();
            Console.WriteLine($"{new string('-', 90)}");
        }

        static void PrintFormat()
        {
            Console.WriteLine($"{new string('-', 33)}All account information{new string('-', 34)}");
            Console.WriteLine($"Consumer ID{new string(' ', 5)}Name{new string(' ', 16)}Account Number{new string(' ', 6)}Type{new string(' ', 12)}Balance");
            Console.WriteLine($"{new string('-', 90)}");
        }

        static void Accounts()
        {
            PrintFormat();
            Bank.AccountList.Add(new SavingsAccount("S647", "Alex Du", 222290192, 4783.98));
            Bank.AccountList.Add(new ChequingAccount("C576", "Dale Stayne", 333312312, 12894.56));

            Bank.ShowAll();

            Console.WriteLine($"{new string('-', 90)}");
            Console.WriteLine("Trying to withdraw $500.00 from the following account");
            Console.WriteLine(Bank.AccountList[0].ToString());
            Bank.AccountList[0].Withdraw(500.00);
            Console.WriteLine($"{new string('-', 90)}");

            Console.WriteLine($"{new string('-', 90)}");
            Console.WriteLine("Trying to deposit $-250.00 to the following account");
            Console.WriteLine(Bank.AccountList[1].ToString());
            Bank.AccountList[1].Deposit(-250.00);
            Console.WriteLine($"{new string('-', 90)}");

            Console.WriteLine($"{new string('-', 90)}");
            Console.WriteLine("Trying to withdraw $10000.00 to the following account");
            Console.WriteLine(Bank.AccountList[2].ToString());
            Bank.AccountList[2].Withdraw(10000.00);
            Console.WriteLine($"{new string('-', 90)}");

            Console.WriteLine($"{new string('-', 90)}");
            Console.WriteLine("Trying to deposit $5000.00 to the following account");
            Console.WriteLine(Bank.AccountList[2].ToString());
            Bank.AccountList[2].Deposit(5000.00);
            Console.WriteLine($"{new string('-', 90)}");

            Console.WriteLine($"{new string('-', 90)}");
            Console.WriteLine("Trying to deposit $7300.90 to the following account");
            Console.WriteLine(Bank.AccountList[3].ToString());
            Bank.AccountList[3].Deposit(7300.90);
            Console.WriteLine($"{new string('-', 90)}");

            Console.WriteLine($"{new string('-', 90)}");
            Console.WriteLine("Trying to withdraw $45000.40 from the following account");
            Console.WriteLine(Bank.AccountList[4].ToString());
            Bank.AccountList[4].Withdraw(45000.40);
            Console.WriteLine($"{new string('-', 90)}");

            Console.WriteLine($"{new string('-', 90)}");
            Console.WriteLine("Trying to withdraw $37000.00 from the following account");
            Console.WriteLine(Bank.AccountList[5].ToString());
            Bank.AccountList[5].Withdraw(37000);
            Console.WriteLine($"{new string('-', 90)}");

            Console.WriteLine($"{new string('-', 90)}");
            Bank.ShowAll(67676767);
            Console.WriteLine($"{new string('-', 90)}");
            Console.WriteLine();
            PrintFormat();
            Bank.ShowAll();
        }
    }

    public class Consumer
    {
        public string Id { get; }
        public string Name { get; }
        public Consumer(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public override string ToString()
        {
            return $"{this.Id,-16}{this.Name,-20}";
        }
    }

    abstract class Account : Consumer
    {
        public int AccountNum { get; }
        public Account(string id, string nm, int accNum) : base(id, nm)
        {
            this.AccountNum = accNum;
        }

        public abstract void Withdraw(double amount);

        public abstract void Deposit(double amount);

        public string checkType(string id)
        {
            if (id[0] == 'S')
            {
                return "Saving";
            }
            else
            {
                return "Chequing";
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}{this.AccountNum,-20}{checkType(this.Id),-16}";
        }
    }

    class InsufficientBalanceException : Exception
    {
        public override string Message => "Account not having enough balance to withdraw.";
    }

    class MinimumBalanceException : Exception
    {
        public override string Message => "You must maintain minimum $3000 balance in Saving account.";
    }

    class IncorrectAmountException : Exception
    {
        public override string Message => "You must provide positive number for amount to be deposited.";

    }

    class OverdraftLimitException : Exception
    {
        public override string Message => "Overdraft Limit exceeded. You can only use $2000 from overdraft.";
    }

    class AccountNotFoundException : Exception
    {
        public override string Message => "Account with given number does not exist.";
    }


    class SavingsAccount : Account
    {
        public double Balance { get; set; }

        public SavingsAccount(string id, string nm, int accNum, double balance = 0.0) : base(id, nm, accNum)
        {
            this.Balance = balance;
        }

        public override void Withdraw(double amount)
        {
            try
            {

                if (this.Balance - amount < 0)
                {
                    throw new InsufficientBalanceException();
                }
                else if (this.Balance - amount < 3000)
                {
                    throw new MinimumBalanceException();
                }
                else
                {
                    this.Balance -= amount;
                    Console.WriteLine($"Successfully withdrawn ${amount} from the account number {this.AccountNum}");
                    Console.WriteLine($"Updated balance is ${Math.Round(this.Balance, 2)}");
                }
            }
            catch (InsufficientBalanceException ibe)
            {
                Console.WriteLine($"{ibe.Message}");
            }
            catch (MinimumBalanceException mbe)
            {
                Console.WriteLine($"{mbe.Message}");
            }
        }

        public override void Deposit(double amount)
        {
            try
            {
                if (amount < 0)
                {
                    throw new IncorrectAmountException();
                }
                else
                {
                    this.Balance += amount;
                    Console.WriteLine($"Successfully deposited ${amount} to the account number {this.AccountNum}");
                    Console.WriteLine($"Updated balance is ${Math.Round(this.Balance, 2)}");
                }
            }
            catch (IncorrectAmountException iae)
            {
                Console.WriteLine($"{iae.Message}");
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}${Math.Round(this.Balance, 2),-1}";
        }
    }

    class ChequingAccount : Account
    {
        public double Balance { get; set; }

        public ChequingAccount(string id, string nm, int accNum, double balance = 0.0) : base(id, nm, accNum)
        {
            this.Balance = balance;
        }

        public override void Withdraw(double amount)
        {
            try
            {
                if (this.Balance - amount + 2000 < 0)
                {
                    throw new OverdraftLimitException();
                }
                else
                {
                    this.Balance -= amount;
                    Console.WriteLine($"Successfully withdrawn ${amount} from the account number {this.AccountNum}");
                    Console.WriteLine($"Updated balance is ${Math.Round(this.Balance, 2)}");
                }
            }
            catch (OverdraftLimitException ole)
            {
                Console.WriteLine($"{ole.Message}");
            }
        }

        public override void Deposit(double amount)
        {
            try
            {
                if (amount < 0)
                {
                    throw new IncorrectAmountException();
                }
                else
                {
                    this.Balance += amount;
                    Console.WriteLine($"Successfully deposited ${amount} to the account number {this.AccountNum}");
                    Console.WriteLine($"Updated balance is ${Math.Round(this.Balance, 2)}");
                }
            }
            catch (IncorrectAmountException iae)
            {
                Console.WriteLine($"{iae.Message}");
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}${Math.Round(this.Balance, 2),-1}";
        }
    }

    class Bank
    {
        public static List<Account> AccountList;

        static Bank()
        {
            AccountList = new List<Account>();
            AccountList.Add(new SavingsAccount("S101", "James Finch", 222210212, 3400.90));
            AccountList.Add(new SavingsAccount("S102", "Kell Forest", 222214500, 42520.32));
            AccountList.Add(new SavingsAccount("S103", "Amy Stone", 222212000, 8273.45));
            AccountList.Add(new ChequingAccount("C104", "Kaitlin Ross", 333315002, 91230.45));
            AccountList.Add(new ChequingAccount("C105", "Adem First", 333303019, 43987.67));
            AccountList.Add(new ChequingAccount("C106", "John Doe", 333358927, 34829.76));
        }

        public static void ShowAll()
        {
            foreach (Account acc in Bank.AccountList)
            {
                Console.WriteLine($"{acc}");
            }
        }

        public static void ShowAll(int accNum)
        {
            Console.WriteLine();
            Console.WriteLine($"{new string('-', 25)}Details of account number {accNum}{new string('-', 31)}");
            bool found = false;
            try
            {
                foreach (Account acc in Bank.AccountList)
                {
                    if (acc.AccountNum == accNum)
                    {
                        Console.WriteLine($"{acc}");
                        found = true;
                    }
                }
                if (!found)
                {
                    throw new AccountNotFoundException();
                }
            }
            catch (AccountNotFoundException anfe)
            {
                Console.WriteLine($"{anfe.Message}");
            }
        }
    }
}
