using System;
using System.Collections.Generic;
using System.Linq;
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
        private IList<Portfolio> _portfolios = new List<Portfolio>();

        public Account()
        {

        }

        public void AddPortfolio(string name)
        {
            if (_portfolios.Count >= MAX_NUMBER_OF_PORTFOLIOS)
            {
                throw new TooManyPortfoliosException();
            }

            if (_portfolios.Any(x => x.Name == name))
            {
                throw new NonUniquePortfolioNameException();
            }

            Portfolio newPortfolio = new Portfolio(name, this);
            _portfolios.Add(newPortfolio);
        }

        public void DeletePortfolio(string name, Dictionary<Ticker, decimal> stockPrices)
        {
            if (_portfolios.Count <= 0)
            {
                throw new TooFewPortfoliosException();
            }

            Portfolio portfolioToRemove = null;
            foreach (Portfolio portfolio in _portfolios)
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
            _portfolios.Remove(portfolioToRemove);
        }

        public override string ToString()
        {
            return string.Format("Account: balance={0}, portfolios={1}", _balance, _portfolios);
        }
    }

    class AccountReport
    {
        public readonly decimal tradeFees;
        public readonly decimal transferFees;
        public readonly decimal currentBalance;
        public readonly decimal currentValue;
        public readonly decimal realizedGains;
        public readonly decimal projectedGains;
        public readonly IDictionary<Ticker, decimal> stockValues;
        public readonly IDictionary<Ticker, decimal> stockPercentages;
        public readonly IDictionary<string, decimal> portfolioValues;
        public readonly IDictionary<string, decimal> portfolioPercentages;

        public AccountReport(decimal tradeFees, decimal transferFees,
            decimal currentBalance, decimal currentValue, decimal realizedGains, decimal projectedGains,
            IDictionary<Ticker, decimal> stockValues, IDictionary<Ticker, decimal> stockPercentages,
            IDictionary<string, decimal> portfolioValues, IDictionary<string, decimal> portfolioPercentages)
        {
            this.tradeFees = tradeFees;
            this.transferFees = transferFees;
            this.currentValue = currentValue;
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
