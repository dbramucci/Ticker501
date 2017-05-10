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
        public List<StockCollection> stocks;

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

        /// <summary>
        /// Adds purchase to stock history.
        /// </summary>
        /// <param name="tick"> The ticker of the stock purchase. </param>
        /// <param name="quantity"> The quantity of the stock to buy. </param>
        /// <param name="prices"> The price of the stock. </param>
        public void BuyStock(Ticker tick, int quantity, Dictionary<Ticker, decimal> prices)
        {
            StockCollection purchase = new StockCollection(tick, quantity, prices[tick]);
            stocks.Add(purchase);
        }

        /// <summary>
        /// Attempts to sell stocks from the portfolio.
        /// </summary>
        /// <exception cref="InsufficientStocksException"> Thrown if there aren't enough stocks to sell. </exception>
        /// <param name="tick"></param>
        /// <param name="quantity"></param>
        /// <param name="prices"></param>
        /// <returns></returns>
        public decimal SellStock(Ticker tick, int quantity, Dictionary<Ticker, decimal> prices)
        {
            if (stocks.Where(x => x.stockTicker.name == tick.name).Sum(x => x.quantity) < quantity)
            {
                throw new InsufficientStocksException();
            }

            decimal gains = 0m;
            int i = 0;

            while (quantity > 0)
            {
                if (stocks[i].stockTicker.name != tick.name)
                {
                    i++;
                }
                if (stocks[i].quantity <= quantity)
                {
                    quantity -= stocks[i].quantity;
                    gains += (prices[tick] - stocks[i].priceAtPurchase) * stocks[i].quantity;
                    stocks.RemoveAt(i); // No need to increment i
                }
                else
                {
                    int newQuantity = stocks[i].quantity - quantity;
                    gains += (prices[tick] - stocks[i].priceAtPurchase) * quantity;
                    stocks[i] = new StockCollection(stocks[i].stockTicker, newQuantity, stocks[i].priceAtPurchase);
                }
            }
            realizedGains += gains;
            return gains;
        }

        public PortfolioReport MakeReport(Dictionary<Ticker, decimal> prices)
        {
            decimal cost = stocks.Select(x => x.priceAtPurchase * x.quantity).Sum();
            decimal possibleProfit = stocks.Select(x => x.quantity * (prices[x.stockTicker] - x.priceAtPurchase)).Sum();

            Dictionary<Ticker, decimal> percentages = new Dictionary<Ticker, decimal>();
            Dictionary<Ticker, decimal> stockValues = new Dictionary<Ticker, decimal>();

            decimal totalValue = stocks.Select(x => x.quantity * prices[x.stockTicker]).Sum();

            foreach (var stock in stocks)
            {
                stockValues[stock.stockTicker] = stock.quantity * prices[stock.stockTicker];
                if (totalValue != 0m)
                {
                    percentages[stock.stockTicker] = stock.quantity * prices[stock.stockTicker] / totalValue;
                }
                else
                {
                    percentages[stock.stockTicker] = 0m;
                }
            }

            return new PortfolioReport(
                realizedGains,
                realizedGains + possibleProfit,
                stockValues,
                percentages);
        }

    }

    class PortfolioReport
    {
        public readonly decimal realizedGains;
        public readonly decimal projectedGains;
        public readonly IDictionary<Ticker, decimal> stockValues;  // In future, may wish to switch with an immutable dictionary.
        public readonly IDictionary<Ticker, decimal> stockPercentages;

        public PortfolioReport(decimal realizedGains, decimal projectedGains, IDictionary<Ticker, decimal> stockValues, IDictionary<Ticker, decimal> stockPercentages)
        {
            this.realizedGains = realizedGains;
            this.projectedGains = projectedGains;
            this.stockValues = stockValues;
            this.stockPercentages = stockPercentages;
        }
    }
}
