using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker501
{
    class Ticker
    {
        public readonly string name;
        public readonly string symbol;

        public Ticker(string symbol, string name)
        {
            this.name = name;
            this.symbol = symbol;
        }

        public override string ToString()
        {
            return string.Format("Ticker({0}, {1})", symbol, name);
        }

        public override bool Equals(Object obj)
        {
            return ToString() == obj.ToString();
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
