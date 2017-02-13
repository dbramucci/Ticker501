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
                int choice = displayMenu(mainMenu);
                if (choice == 0)
                {
                    ShowAccountReport();
                }
                else  if (choice == 1)
                {
                    ShowPortfolioReport();
                }
                else if (choice == 2)
                {
                    AddFunds();
                }
                else if (choice == 3)
                {
                    WithdrawFunds();
                }
                else if (choice == 4)
                {
                    BuyStock();
                }
                else if (choice == 5)
                {
                    SellStock();
                }
                else if (choice == 6)
                {
                    MakePortfolio();
                }
                else if (choice == 7)
                {
                    DeletePortfolio();
                }
                else if (choice == 8)
                {
                    break;
                }
            }
        }

        static int displayMenu(string[] options)
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

        private static void ShowAccountReport()
        {

        }

        private static void ShowPortfolioReport()
        {

        }
        private static void AddFunds()
        {

        }
        private static void WithdrawFunds()
        {

        }
        private static void BuyStock()
        {

        }
        private static void SellStock()
        {

        }
        private static void MakePortfolio()
        {

        }
        private static void DeletePortfolio() { }
    }
}
