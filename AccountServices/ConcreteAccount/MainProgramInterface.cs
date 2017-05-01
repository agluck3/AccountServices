using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using AccountHandler;

namespace ConcreteAccount
{
    class MainProgramInterface
    {
        private CustomBank bank = new CustomBank();
        public void Initialize(string[] args)
        {
            if (args.Length == 4)
            {
                SetupLogging(args[3]);
                GetAccountsAndBalancesFromFile(args[1]);
                ProcessTransactionsFromSecondFile(args[0]);
                OutputAccountStateToThirdFile(args[2]);

                /*
                private Bank bank = new Bank(file openingBalances);
                bank.importTransactions(file transactionFile);
                string report=bank.printAccountReport();
                 */

                Console.WriteLine("File Processing is complete. Press any key to exit.");
            }
            else displayHelp();

            Console.Read();
        }

        private void SetupLogging(string fileName)
        {
            string logFilePath = fileName;
            FileStream hlogFile = new FileStream(logFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            TextWriterTraceListener myListener = new TextWriterTraceListener(hlogFile);
            Trace.Listeners.Add(myListener);
            Trace.AutoFlush = true;
        }

        private void GetAccountsAndBalancesFromFile(String fileName)
        {
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                try
                {
                    Console.WriteLine("Loading Account Data into memory . . .");
                    int count=0;
                    while (!streamReader.EndOfStream)
                    {
                        string account = streamReader.ReadLine();
                        string[] accountData = account.Split(',');
                        int accountNumber = int.Parse(accountData[0]);
                        string accountHolderName = accountData[1];
                        float balance = float.Parse(accountData[2]);
                        Console.WriteLine($"Adding account #{count++}: {accountNumber}");
                        bank.addAccount(accountData[3], accountNumber, accountHolderName, balance);
                    }                    
               }
                catch (Exception ex) { Console.WriteLine("Exception: " + ex.Message); }
                finally { streamReader.Close(); }
                Console.WriteLine($"There are {bank.getAccountList().Count}");
            }
        }

        private void ProcessTransactionsFromSecondFile(String fileName)
        {
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                try
                {
                    Console.WriteLine("Processing Transactions . . .");
                    while (!streamReader.EndOfStream)
                    {
                        try
                        {
                            string transaction = streamReader.ReadLine();
                            //The comments for here will pretty well be the same as the accountFile bellow
                            //Nothing special going on in here. Basic data handeling and list populaiton.
                            string[] transactionData = transaction.Split(',');
                            Account sourceAccount = bank.getAccount(int.Parse(transactionData[0]));
                            Account destinationAccount = bank.getAccount(int.Parse(transactionData[1]));
                            float transactionAmount = float.Parse(transactionData[2]);

                            try
                            {
                                sourceAccount.debit(transactionAmount);
                                destinationAccount.credit(transactionAmount);
                            }
                            catch (FallsBelowMinimumException fbme)
                            {
                                Trace.WriteLine(fbme.Message);
                            }
                        }
                        catch (FormatException fe) { Console.WriteLine("Format Exception: " + fe.Message); }
                        catch (Exception ex) { Console.WriteLine("Exception: " + ex.Message); }
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                finally { streamReader.Close(); }
            }
        }
        public void OutputAccountStateToThirdFile(string fileName)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(fileName, false);
                List<Account> accountList = bank.getAccountList();
                accountList.Sort();
                foreach (Account acct in accountList)
                {
                    //10165, Temptexon, 17283.40, Business
                    streamWriter.WriteLine($"{acct.accountNumber}, { acct.accountHolderName}, {acct.balance:.00}, {acct.accountType}");
                }
                streamWriter.Close();
            }
            catch (Exception ex) { Console.WriteLine("An error occured accessing the outputFile: " + ex.Message); }
            finally { }
        }

        public void displayHelp()
        { 
            Console.WriteLine(@"Usage: IngenAccountServices [TranactionFile] [AccountsFile] [OutputFile] [ErrorFile]");
        }
    }
}
