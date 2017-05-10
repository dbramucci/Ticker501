using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker501
{
    class Program
    {
        static void Main(string[] args)
        {
            Account mainAccount = new Account();
            Console.WriteLine("Welcome to Ticker501!");
            Console.Write("Please select a ticker file: ");
            string tickerFileName = Console.ReadLine();

            Tuple<List<Ticker>, Dictionary<Ticker, decimal>> data = TickerReader.ReadTickerFile(tickerFileName);

            var tickers = data.Item1;
            var prices = data.Item2;


            string[] mainMenu = new string[]
            {
                "Show Account Report",
                "Show Portfolio Report",
                "Add Funds to Account",
                "Withdraw Funds From Account",
                "Buy Stocks",
                "Sell Stocks",
                "Create Portfolio",
                "Delete Portfolio",
                "Quit"
            };
            while (true)
            {
                int choice = DisplayMenu(mainMenu);
                if (choice == 0)
                {
                    //ShowAccountReport();
                }
                else  if (choice == 1)
                {
                    //ShowPortfolioReport();
                }
                else if (choice == 2)
                {
                    AddFunds(mainAccount);
                }
                else if (choice == 3)
                {
                    WithdrawFunds(mainAccount);
                }
                else if (choice == 4)
                {
                    BuyStock(tickers, prices);
                }
                else if (choice == 5)
                {
                    SellStock();
                }
                else if (choice == 6)
                {
                    MakePortfolio(mainAccount);
                }
                else if (choice == 7)
                {
                    DeletePortfolio(mainAccount, prices);
                }
                else if (choice == 8)
                {
                    break;
                }
            }
        }

        static int DisplayMenu(string[] options)
        {
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine("[{0}] - {1}", i + 1, options[i]);
            }
            Console.WriteLine("Please enter the number corresponding with your choice.");
            int? userChoice = null;
            while (userChoice == null)
            {
                try
                {
                    userChoice = Convert.ToInt32(Console.ReadLine());
                    if (0 < userChoice && userChoice <= options.Length)
                    {
                        
                    }
                    else
                    {
                        Console.WriteLine("Please enter an int shown: ");
                        userChoice = null;
                    }
                }
                catch
                {
                    Console.WriteLine("Sorry, I didn't recognize that, Please enter an int: ");
                }
            }
            return (int) userChoice;
        }

        private static void ShowAccountReport(AccountReport report)
        {
            Console.WriteLine("Account Report");

            Console.WriteLine("Current Balance = {0}", report.currentBalance);

            if (report.projectedGains >= 0)
            {
                Console.WriteLine("Projected Gains = {0}", report.projectedGains);
            }
            else
            {
                Console.WriteLine("Projected Losses = {0}", -report.projectedGains);
            }

            if (report.realizedGains >= 0)
            {
                Console.WriteLine("Realized Gains = {0}", report.realizedGains);
            }
            else
            {
                Console.WriteLine("Realized Losses = {0}", -report.realizedGains);
            }

            Console.WriteLine("Stock - Percentage");
            foreach (var data in report.stockPercentages.OrderByDescending(x => x.Value))
            {
                Console.WriteLine("{0} - {1}", data.Key, data.Value);
            }

            Console.WriteLine("Stock - Dollar");
            foreach (var data in report.stockValues.OrderByDescending(x => x.Value))
            {
                Console.WriteLine("{0} - {1}", data.Key, data.Value);
            }

            Console.WriteLine("Portfolio - Percentage");
            foreach (var data in report.portfolioPercentages.OrderByDescending(x => x.Value))
            {
                Console.WriteLine("{0} - {1}", data.Key, data.Value);
            }

            Console.WriteLine("Portfolio - Dollar");
            foreach (var data in report.portfolioValues.OrderByDescending(x => x.Value))
            {
                Console.WriteLine("{0} - {1}", data.Key, data.Value);
            }
        }

        private static void ShowPortfolioReport(PortfolioReport report)
        {
            Console.WriteLine("Account Report");

            if (report.projectedGains >= 0)
            {
                Console.WriteLine("Projected Gains = {0}", report.projectedGains);
            }
            else
            {
                Console.WriteLine("Projected Losses = {0}", -report.projectedGains);
            }

            if (report.realizedGains >= 0)
            {
                Console.WriteLine("Realized Gains = {0}", report.realizedGains);
            }
            else
            {
                Console.WriteLine("Realized Losses = {0}", -report.realizedGains);
            }

            Console.WriteLine("Stock - Percentage");
            foreach (var data in report.stockPercentages.OrderByDescending(x => x.Value))
            {
                Console.WriteLine("{0} - {1}", data.Key, data.Value);
            }

            Console.WriteLine("Stock - Dollar");
            foreach (var data in report.stockValues.OrderByDescending(x => x.Value))
            {
                Console.WriteLine("{0} - {1}", data.Key, data.Value);
            }
        }


        private static void AddFunds(Account account)
        {
            Console.WriteLine("Please enter the amount of funds that you would like as a decimal (1.25): ");
            string response = Console.ReadLine();
            decimal amount = Convert.ToDecimal(response);
            account.AddFunds(amount);
        }

        private static void WithdrawFunds(Account account)
        {
            Console.WriteLine("Please enter the amount of funds to withdraw as a decimal (1.25): ");
            string response = Console.ReadLine();
            decimal amount = Convert.ToDecimal(response);
            try
            {
                account.Withdrawfunds(amount);
                Console.WriteLine($"You withdrawed {amount}");
            }
            catch (InsufficientFundsException)
            {
                Console.WriteLine("You have insufficient funds to do this so your account is unchanged.");
            }
        }
        private static void BuyStock(List<Ticker> tickers, Dictionary<Ticker, decimal> prices)
        {
            Console.Write("What portfolio will you use?");
            string name = Console.ReadLine();

            Ticker ticker = null;
            while (ticker == null)
            {
                Console.Write("Ticker name/symbol: ");
                string tickerNameOrSymbol = Console.ReadLine();
                for (int i = 0; i < tickers.Count && ticker == null ; i++)
                {
                    if (tickers[i].symbol == tickerNameOrSymbol || tickers[i].name == tickerNameOrSymbol)
                    {
                        ticker = tickers[i];
                    }
                }
                if (ticker == null)
                {
                    Console.WriteLine("Error, could not find ticker with name or symbol {0}", tickerNameOrSymbol);
                }
            }
        }
        private static void SellStock()
        {

        }
        private static void MakePortfolio(Account account)
        {
            Console.Write("What's the name of your new portfolio");
            string name = Console.ReadLine();
            try
            {
                account.AddPortfolio(name);
            }
            catch (TooManyPortfoliosException)
            {
                Console.WriteLine("Sorry, you have too many portfolios to add a new portfolio.");
                Console.WriteLine("Adding new Portfolio has been aborted");
            }
        }

        private static void DeletePortfolio(Account account, Dictionary<Ticker, decimal> prices)
        {
            Console.Write("What portfolio would you like to delete: ");
            string name = Console.ReadLine();
            try
            {
                account.DeletePortfolio(name, prices);
            }
            catch (TooManyPortfoliosException)
            {
                Console.WriteLine("Sorry, you have don't have any portfolios to delete");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Sorry, I could find any portfolios with the name: {0}", name);
            }
        }
    }
}
