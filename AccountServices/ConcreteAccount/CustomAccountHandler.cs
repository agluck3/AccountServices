using AccountHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteAccount {

    public class CustomBank : Bank {

        public override void addAccount(string type, int accountNumber, string accountHolderName, float balance) {
            if (type.Contains("Business"))
                this.addAccountToList(new BusinessAccount(accountNumber, accountHolderName, balance));
            else if (type.Contains("Personal"))
                this.addAccountToList(new PersonalAccount(accountNumber, accountHolderName, balance));
            else throw new InvalidAccountTypeException($"[-] Missing account type on account {accountNumber}");
        }


    }
    public class PersonalAccount : Account
    {
        public PersonalAccount(int accountNumber, string accountHolderName, float balance)
            : base(accountNumber, accountHolderName, balance) { }
    }

    public class BusinessAccount : Account
    {
        private const int BUSINESS_OVERDRAFT_CAP = -1000;
        private const float BUSINESS_FEE = 1.00f;
        private const float BUSINESS_OVERDRAFT_FEE = 20.00f;

        public BusinessAccount(int accountNumber, string accountHolderName, float balance) :
            base(accountNumber, accountHolderName, balance)
        { this.accountType = "Business"; }


        public override void debit(float amount)
        {
            float testBalance = balance - amount - BUSINESS_FEE;
            if (testBalance < BUSINESS_OVERDRAFT_CAP)
            {
                balance -= BUSINESS_OVERDRAFT_FEE;
                throw new FallsBelowMinimumException("Transaction for account " + accountNumber.ToString() + " would reduce balance to " +
                                                        testBalance.ToString() + " A $20.00 fee has been placed on the account, the new balance is: " +
                                                        balance.ToString());
            }
            else balance = testBalance;
        }

        public override void credit(float amount) { balance += amount; }
    }


}
