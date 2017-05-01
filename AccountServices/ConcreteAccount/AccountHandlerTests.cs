using ConcreteAccount;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountHandler
{
    [TestFixture]
    class AccountHandlerTests
    {
        [Test]
        public void addAccount_standardPersonal_shouldShowUpInList() {
            CustomBank myBank = new CustomBank();
            PersonalAccount myAcct = new PersonalAccount(123, "Walker", 1000000);
            myBank.addAccount(myAcct);
            CollectionAssert.Contains(myBank.getAccountList(), myAcct);
        }
        [Test]
        public void addAccount_standardPersonal_OpenBalChk()
        {
            CustomBank myBank = new CustomBank();
            PersonalAccount myAcct = new PersonalAccount(123, "Walker", 1000000);
            myBank.addAccount(myAcct);
            Assert.AreEqual(1000000,myBank.getAccount(123).balance);
        }
        [Test]
        public void debit_standardPersonal_add100()
        {
            CustomBank myBank = new CustomBank();
            PersonalAccount myAcct = new PersonalAccount(123, "Walker", 1000000);
            myBank.addAccount(myAcct);
            myAcct.credit(100);
            Assert.AreEqual(1000100, myBank.getAccount(123).balance);
        }
    }

}
