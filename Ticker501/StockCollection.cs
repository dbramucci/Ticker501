using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker501
{
    class StockCollection
    {
        public readonly Ticker stockTicker;
        public readonly int quantity;
        public readonly decimal priceAtPurchase;

        public StockCollection(Ticker stockTicker, int quantity, decimal priceAtPurchase)
        {
            this.stockTicker = stockTicker;
            this.quantity = quantity;
            this.priceAtPurchase = priceAtPurchase;
        }

        public override string ToString()
        {
            return string.Format("Ticker({}, {}, {})", stockTicker, quantity, priceAtPurchase);
        }
    }
}
