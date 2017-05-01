//Account handler is resposible for recieving data from the file handler, processing that data, 
//and then sending the processed data back to the file handler. 
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountHandler
{
    public abstract class Bank {
        private Dictionary<string,Type> accountTypes = new Dictionary<string,Type>();
        public void addAccountType(string id,Type type) { accountTypes.Add(id,type);  }
        private Dictionary<int, Account> Accounts = new Dictionary<int, Account>();
        virtual public void addAccount(string type, int accountNumber, string accountHolderName, float balance) { }
        virtual public void addAccount(Account account) { addAccountToList(account); }
        protected void addAccountToList(Account account) { Accounts.Add(account.accountNumber, account); }
        public Account getAccount(int key) { return Accounts[key]; }
        public List<Account> getAccountList() { return Accounts.Values.ToList(); }
    }

    public abstract class Account : IComparable<Account> {
        public int accountNumber { get; set; }
        public string accountHolderName { get; set; }
        public float balance { get; set; }
        public string accountType;
        private const int ZERO_BALANCE = 0;
        
        public Account(int accountNumber, string accountHolderName, float balance)
        {
            this.accountNumber = accountNumber;
            this.accountHolderName = accountHolderName;
            this.balance = balance;
            this.accountType = "Personal";
        }
        public virtual void debit(float amount) {
            float testBalance = balance - amount;
            if (testBalance < ZERO_BALANCE) {
                throw new FallsBelowMinimumException("Transaction for account " + accountNumber.ToString() + " would reduce balance to " +
                                                      testBalance.ToString() + " The transaction was not processed");
            }
            else { balance = testBalance; }
        }
        public virtual void credit(float amount) { balance += amount; }
        public int CompareTo(Account other) {
            return this.accountNumber.CompareTo(other.accountNumber);
        }
    }
    public class FallsBelowMinimumException : Exception {
        //Good practices to have the same constructors avaliable here that the base class has
        //Just set them to inherit the base class. 
        public FallsBelowMinimumException() { }
        public FallsBelowMinimumException(string message) : base(message) { }
        public FallsBelowMinimumException(string message, Exception inner) : base(message, inner) { }
    }
    public class InvalidAccountTypeException : Exception {
        //Good practices to have the same constructors avaliable here that the base class has
        //Just set them to inherit the base class. 
        public InvalidAccountTypeException() { }
        public InvalidAccountTypeException(string message) : base(message) { }
        public InvalidAccountTypeException(string message, Exception inner) : base(message, inner) { }
    }
}