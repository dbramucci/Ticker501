using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker501
{
    class Portfolio
    {
        private string name;
        private Account owningAccount;
        private decimal realizedGains = 0m;
        private List<StockCollection> stocks;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public Portfolio(string name, Account owningAccount)
        {
            this.name = name;
            this.owningAccount = owningAccount;
            stocks = new List<StockCollection>();
        }

        public decimal Value(IDictionary<Ticker, decimal> prices)
        {
            decimal value = 0m;

            foreach (StockCollection stockGroup in stocks)
            {
                value += stockGroup.quantity * prices[stockGroup.stockTicker];
            }

            return value;
        }

        public decimal Cleanup(IDictionary<Ticker, decimal> prices)
        {
            decimal losses = 0m;

            foreach (StockCollection stockGroup in stocks)
            {
                losses += stockGroup.quantity * stockGroup.priceAtPurchase;
            }

            realizedGains -= losses;

            realizedGains += Value(prices);

            stocks = null;

            return realizedGains;
        }

    }

    class PortfolioReport
    {
        public readonly decimal currentValue;
        public readonly decimal realizedGains;
        public readonly decimal projectedGains;
        public readonly IDictionary<Ticker, decimal> stockValues;  // In future, may wish to switch with an immutable dictionary.
        public readonly IDictionary<Ticker, decimal> stockPercentages;

        public PortfolioReport(decimal currentValue, decimal realizedGains, decimal projectedGains, IDictionary<Ticker, decimal> stockValues, IDictionary<Ticker, decimal> stockPercentages)
        {
            this.currentValue = currentValue;
            this.realizedGains = realizedGains;
            this.projectedGains = projectedGains;
            this.stockValues = stockValues;
            this.stockPercentages = stockPercentages;
        }
    }
}
