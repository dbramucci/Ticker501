using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ticker501
{
    class Account
    {
        private const int MAX_NUMBER_OF_PORTFOLIOS = 3;
        private const decimal TRADE_FEE = 9.99m;
        private const decimal TRANSFER_FEE = 4.99m;

        private decimal _balance = 0m;
        private decimal _tradeFees = 0m;
        private decimal _transferFees = 0m;
        private decimal realizedGains = 0m;
        public IList<Portfolio> Portfolios = new List<Portfolio>();

        public Account()
        {

        }

        /// <summary>
        /// Adds a portfolio to the account with the name name.
        /// </summary>
        /// <exception cref="TooManyPortfoliosException"></exception>
        /// <exception cref="NonUniquePortfolioNameException"></exception>
        /// <param name="name"> The name of the new portfolio. (Must be unique) </param>
        public void AddPortfolio(string name)
        {
            if (Portfolios.Count >= MAX_NUMBER_OF_PORTFOLIOS)
            {
                throw new TooManyPortfoliosException();
            }

            if (Portfolios.Any(x => x.Name == name))
            {
                throw new NonUniquePortfolioNameException();
            }

            Portfolio newPortfolio = new Portfolio(name, this);
            Portfolios.Add(newPortfolio);
        }

        /// <summary>
        /// Deletes a portfolio
        /// </summary>
        /// <exception cref="TooFewPortfoliosException"> If there are zero porfolios this exception is thrown. </exception>
        /// <exception cref="ArgumentException"> If the name doesn't match and portfolios this is thrown. </exception>
        /// <param name="name"> The portfolio to delete. </param>
        /// <param name="stockPrices"> The current stock prices. </param>
        public void DeletePortfolio(string name, Dictionary<Ticker, decimal> stockPrices)
        {
            if (Portfolios.Count <= 0)
            {
                throw new TooFewPortfoliosException();
            }

            Portfolio portfolioToRemove = null;
            foreach (Portfolio portfolio in Portfolios)
            {
                if (name == portfolio.Name)
                {
                    portfolioToRemove = portfolio;
                }
            }
            if (portfolioToRemove == null)
            {
                throw new ArgumentException("Name not in portfolios");
            }
            realizedGains += portfolioToRemove.Cleanup(stockPrices);
            Portfolios.Remove(portfolioToRemove);
        }

        /// <summary>
        /// Adds additionalFunds to the account.
        /// </summary>
        /// <param name="additionalFunds"> The amount being transferred in. </param>
        public void AddFunds(decimal additionalFunds)
        {
            _balance += additionalFunds;
            _transferFees += TRANSFER_FEE;
        }

        /// <summary>
        /// Withdraw a certain amount of funds from the account. 
        /// </summary>
        /// <exception cref="InsufficientFundsException"> Thrown if the user doesn't have enogh money to withdraw. </exception>
        /// <param name="withdrawAmount"> The amount to withdraw. </param>
        public void Withdrawfunds(decimal withdrawAmount)
        {
            decimal cost = withdrawAmount + TRANSFER_FEE;
            if (_balance < cost)
            {
                throw new InsufficientFundsException();
            }
            _balance -= cost;
            _transferFees += TRANSFER_FEE;
        }

        /// <summary>
        /// Buys stocks
        /// </summary>
        /// <returns></returns>
        public decimal BuyStocks()
        {
            return 0m;
        }

        /// <summary>
        /// Makes a report to be displayed.
        /// </summary>
        /// <returns> The report of the account. </returns>
        public AccountReport MakeReport(Dictionary<Ticker, decimal> prices)
        {
            decimal projectedGains = 0m;

            decimal stockValueSum =
                Portfolios.SelectMany(x => x.stocks.Select(y => y.quantity * prices[y.stockTicker])).Sum();

            Dictionary<Ticker, decimal> percentages = new Dictionary<Ticker, decimal>();
            Dictionary<Ticker, decimal> stockValues = new Dictionary<Ticker, decimal>();

            foreach (StockCollection stock in Portfolios.SelectMany(x => x.stocks))
            {
                Ticker tick = stock.stockTicker;
                stockValues[tick] = stock.quantity * prices[tick];
                if (stockValueSum != 0m)
                {
                    percentages[tick] = stockValues[tick] / stockValueSum;
                }
                else
                {
                    percentages[tick] = 0m;
                }
            }

            Dictionary<string, decimal> portfolioValues = new Dictionary<string, decimal>();
            Dictionary<string, decimal> portfolioPercentages = new Dictionary<string, decimal>();

            decimal totalPortfolioValue = Portfolios.Select(x => x.Value(prices)).Sum();
            foreach (Portfolio port in Portfolios)
            {
                decimal value = port.Value(prices);
                portfolioValues[port.Name] = value;
                if (totalPortfolioValue == 0m)
                {
                    portfolioPercentages[port.Name] = value / totalPortfolioValue;
                }
                else
                {
                    portfolioPercentages[port.Name] = 0m; // Technically its undefined, but people generally prefer to see 0%.
                }
            }


            return new AccountReport(
                _tradeFees,
                _transferFees,
                _balance,
                realizedGains,
                projectedGains,
                stockValues, 
                percentages,
                portfolioValues,
                portfolioPercentages);
        }

        public override string ToString()
        {
            return string.Format("Account: balance={0}, portfolios={1}", _balance, Portfolios);
        }
    }

    class AccountReport
    {
        public readonly decimal tradeFees;
        public readonly decimal transferFees;
        public readonly decimal currentBalance;
        public readonly decimal realizedGains;
        public readonly decimal projectedGains;
        public readonly IDictionary<Ticker, decimal> stockValues;
        public readonly IDictionary<Ticker, decimal> stockPercentages;
        public readonly IDictionary<string, decimal> portfolioValues;
        public readonly IDictionary<string, decimal> portfolioPercentages;

        public AccountReport(decimal tradeFees, decimal transferFees,
            decimal currentBalance,  decimal realizedGains, decimal projectedGains,
            IDictionary<Ticker, decimal> stockValues, IDictionary<Ticker, decimal> stockPercentages,
            IDictionary<string, decimal> portfolioValues, IDictionary<string, decimal> portfolioPercentages)
        {
            this.tradeFees = tradeFees;
            this.transferFees = transferFees;
            this.realizedGains = realizedGains;
            this.projectedGains = projectedGains;
            this.stockValues = stockValues;
            this.stockPercentages = stockPercentages;
            this.currentBalance = currentBalance;
            this.portfolioValues = portfolioValues;
            this.portfolioPercentages = portfolioPercentages;
        }
    }
}
