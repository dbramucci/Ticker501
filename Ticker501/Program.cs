using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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

            if (data == null)
            {
                // File not found and must quit.
                Console.ReadLine();
                return;
            }

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
                "Simulate Market",
                "Quit"
            };
            while (true)
            {
                int choice = DisplayMenu(mainMenu);
                if (choice == 0)
                {
                    ShowAccountReport(mainAccount.MakeReport(prices));
                }
                else  if (choice == 1)
                {
                    var port = ChoosePortfolio(mainAccount);
                    if (port != null)
                    {
                        ShowPortfolioReport(port.MakeReport(prices));
                    }
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
                    BuyStock(mainAccount, tickers, prices);
                }
                else if (choice == 5)
                {
                    Portfolio portfolio = ChoosePortfolio(mainAccount);
                    if (portfolio != null)
                    {
                        SellStock(mainAccount, portfolio, prices, tickers);
                    }
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
                    prices = SimulateMarket(prices);
                }
                else if (choice == 9)
                {
                    break;
                }
            }
        }


        /// <summary>
        /// Allows the user to choose a portfolio.
        /// </summary>
        /// <param name="account"> The account with portfolios to choose from. </param>
        /// <returns> The portfolio choosen or null if none were successfully choosen. </returns>
        static Portfolio ChoosePortfolio(Account account)
        {
            if (account.Portfolios.Count > 0)
            {
                Console.WriteLine("Please enter the name of the portfolio you would like.");
                foreach (Portfolio port in account.Portfolios)
                {
                    Console.WriteLine($"\t{port.Name}");
                }
                string portfolioName = Console.ReadLine();
                try
                {
                    return account.Portfolios.First(x => x?.Name == portfolioName);
                }
                catch
                {
                    Console.WriteLine($"I could not find a portfolio by the name \"{portfolioName}\"");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Sorry, you have no portfolios to display.");
                return null;
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
            return (int) userChoice - 1;
        }

        private static void ShowAccountReport(AccountReport report)
        {
            Console.WriteLine("Account Report");

            Console.WriteLine("Current Balance = {0:C}", report.currentBalance);

            if (report.projectedGains >= 0)
            {
                Console.WriteLine("Projected Gains = {0:C}", report.projectedGains);
            }
            else
            {
                Console.WriteLine("Projected Losses = {0:C}", -report.projectedGains);
            }

            if (report.realizedGains >= 0)
            {
                Console.WriteLine("Realized Gains = {0:C}", report.realizedGains);
            }
            else
            {
                Console.WriteLine("Realized Losses = {0:C}", -report.realizedGains);
            }

            Console.WriteLine($"Transfer Fees = {report.transferFees:C}");
            Console.WriteLine($"Trade Fees = {report.tradeFees:C}");

            Console.WriteLine("Stock - Percentage");
            foreach (var data in report.stockPercentages.OrderByDescending(x => x.Value))
            {
                Console.WriteLine("{0} - {1:P}", data.Key, data.Value);
            }

            Console.WriteLine("Stock - Dollar");
            foreach (var data in report.stockValues.OrderByDescending(x => x.Value))
            {
                Console.WriteLine("{0} - {1:C}", data.Key, data.Value);
            }

            Console.WriteLine("Portfolio - Percentage");
            foreach (var data in report.portfolioPercentages.OrderByDescending(x => x.Value))
            {
                Console.WriteLine("{0} - {1:P}", data.Key, data.Value);
            }

            Console.WriteLine("Portfolio - Dollar");
            foreach (var data in report.portfolioValues.OrderByDescending(x => x.Value))
            {
                Console.WriteLine("{0} - {1:C}", data.Key, data.Value);
            }
        }

        private static void ShowPortfolioReport(PortfolioReport report)
        {
            Console.WriteLine("Account Report");

            if (report.projectedGains >= 0)
            {
                Console.WriteLine("Projected Gains = {0:C}", report.projectedGains);
            }
            else
            {
                Console.WriteLine("Projected Losses = {0:C}", -report.projectedGains);
            }

            if (report.realizedGains >= 0)
            {
                Console.WriteLine("Realized Gains = {0:C}", report.realizedGains);
            }
            else
            {
                Console.WriteLine("Realized Losses = {0:C}", -report.realizedGains);
            }

            Console.WriteLine("Stock - Percentage");
            foreach (var data in report.stockPercentages.OrderByDescending(x => x.Value))
            {
                Console.WriteLine("{0} - {1:P}", data.Key, data.Value);
            }

            Console.WriteLine("Stock - Dollar");
            foreach (var data in report.stockValues.OrderByDescending(x => x.Value))
            {
                Console.WriteLine("{0} - {1:C}", data.Key, data.Value);
            }
        }


        private static void AddFunds(Account account)
        {
            Console.WriteLine("Please enter the amount of funds that you would like as a decimal (1.25): ");
            string response = Console.ReadLine();
            try
            {
                decimal amount = Convert.ToDecimal(response);
                account.AddFunds(amount);
            }
            catch (FormatException)
            {
                Console.WriteLine("That wasn't a decimal, no action will be taken");
            }
        }

        private static void WithdrawFunds(Account account)
        {
            Console.WriteLine("Please enter the amount of funds to withdraw as a decimal (1.25): ");
            string response = Console.ReadLine();
            decimal amount;
            try
            {
                amount = Convert.ToDecimal(response);
            }
            catch (FormatException)
            {
                Console.WriteLine("That wasn't a decimal, no action will be taken");
                return;
            }
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
        private static void BuyStock(Account account, List<Ticker> tickers, Dictionary<Ticker, decimal> prices)
        {
            
            Portfolio portfolio = ChoosePortfolio(account);

            if (portfolio == null)
            {
                return;
            }

            Ticker ticker = null;
            while (ticker == null)
            {
                Console.Write("Ticker name/symbol: ");
                string tickerNameOrSymbol = Console.ReadLine();
                foreach (Ticker t in tickers)
                {
                    if (t.symbol.Trim() == tickerNameOrSymbol.Trim() || t.name.Trim() == tickerNameOrSymbol.Trim())
                    {
                        ticker = t;
                        break;
                    }
                }
                if (ticker == null)
                {
                    Console.WriteLine("Error, could not find ticker with name or symbol {0}", tickerNameOrSymbol);
                }
            }
            int quantity;
            try
            {
                Console.WriteLine("How many of this stock would you like to buy");
                quantity = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Sorry, That isn't a valid number, returning to main menu.");
                return;
            }

            try
            {
                decimal cost = account.BuyStocks(portfolio, quantity, ticker, prices);
                Console.WriteLine($"That costed you {cost:C}, your current balance is {account.MakeReport(prices).currentBalance:C}");
            }
            catch (InsufficientFundsException)
            {
                Console.WriteLine("You have insufficient funds to buy those stocks.");
            }
        }

        private static void SellStock(Account account, Portfolio portfolio, Dictionary<Ticker, decimal> prices, List<Ticker> tickers)
        {
            if (portfolio == null)
            {
                return;
            }

            Ticker ticker = null;
            while (ticker == null)
            {
                Console.Write("Ticker name/symbol: ");
                string tickerNameOrSymbol = Console.ReadLine();
                foreach (Ticker t in tickers)
                {
                    if (t.symbol.Trim() == tickerNameOrSymbol.Trim() || t.name.Trim() == tickerNameOrSymbol.Trim())
                    {
                        ticker = t;
                        break;
                    }
                }
                if (ticker == null)
                {
                    Console.WriteLine("Error, could not find ticker with name or symbol {0}", tickerNameOrSymbol);
                }
            }
            int quantity;
            try
            {
                Console.WriteLine("How many of this stock would you like to sell");
                quantity = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Sorry, That isn't a valid number, returning to main menu.");
                return;
            }

            try
            {
                decimal earnings = account.SellStocks(portfolio, quantity, ticker, prices);
                Console.WriteLine($"That earned you {earnings:C}, your current balance is {account.MakeReport(prices).currentBalance:C}");
            }
            catch (InsufficientStocksException)
            {
                Console.WriteLine("You don't have enough of that stock.");
            }
        }

        private static void MakePortfolio(Account account)
        {
            Console.Write("What's the name of your new portfolio: ");
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
            catch (NonUniquePortfolioNameException)
            {
                Console.WriteLine($"Sorry, you already have a portfolio with the name \"{name}\".");
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
                Console.WriteLine("Sorry, you have don't have any portfolios to delete.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Sorry, I could find any portfolios with the name: {0}", name);
            }
        }

        private static Dictionary<Ticker, decimal> SimulateMarket(Dictionary<Ticker, decimal> prices)
        {
            Console.WriteLine("Select a volitility");
            string[] options = new[]
            {
                "High Volatility (3%-15%)",
                "Medium Volatility (2%-8%)",
                "Low Volatility (1%-4%)"
            };
            int choice = DisplayMenu(options);
            decimal low = 0m;
            decimal high = 0m;
            switch (choice)
            {
                case 0:
                    low = 0.03m;
                    high = 0.15m;
                    break;
                case 1:
                    low = 0.02m;
                    high = 0.08m;
                    break;
                case 2:
                    low = 0.1m;
                    high = 0.04m;
                    break;
            }

            Random rng = new Random();
            var new_prices = new Dictionary<Ticker, decimal>();
            foreach (Ticker tick in prices.Keys)
            {
                int inc = Math.Round(rng.NextDouble()) == 1 ? -1 : 1;
                decimal amount = (Decimal) rng.NextDouble() * (high - low) + low;
                new_prices[tick] = prices[tick] + inc * amount;
            }
            return new_prices;
        }
    }
}
