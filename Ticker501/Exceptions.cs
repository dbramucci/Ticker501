using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker501
{
    class Ticker501Exception : Exception { }
    class TooManyPortfoliosException : Ticker501Exception { }
    class TooFewPortfoliosException : Ticker501Exception { }
    class NonUniquePortfolioNameException : Ticker501Exception { }
    class InsufficientFundsException : Ticker501Exception { }
}
