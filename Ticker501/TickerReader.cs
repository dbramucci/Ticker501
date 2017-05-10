using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker501
{
    class TickerReader
    {
        public static Tuple<List<Ticker>, Dictionary<Ticker, decimal>> ReadTickerFile(string fileName)
        {

            List<Ticker> tickers = new List<Ticker>();
            Dictionary<Ticker, decimal> tickerPrices = new Dictionary<Ticker, decimal>();

            try
            {
                using (StreamReader file = new StreamReader(fileName))
                {
                    string line = file.ReadLine();
                    while (line != null)
                    {
                        string[] lineSplitup = line.Split('-');
                        if (lineSplitup.Length == 3)
                        {
                            string tickerSymbol = lineSplitup[0];
                            string tickerName = lineSplitup[1];
                            decimal tickerPrice = Convert.ToDecimal(lineSplitup[2].Substring(1));

                            Ticker ticker = new Ticker(tickerSymbol, tickerName);
                            tickerPrices.Add(ticker, tickerPrice);
                        }

                        line = file.ReadLine();
                    }
                }
                return new Tuple<List<Ticker>, Dictionary<Ticker, decimal>>(tickers, tickerPrices);
            }
            catch
            {
                Console.WriteLine("Sorry, that file wasn't found. Quiting now");
                return null;
            }
        }
    }
}
