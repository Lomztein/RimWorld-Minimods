using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Lomzie.ProducedLocallyFilter
{
    public class ProductionEntry
    {
        public ThingDef Product { get; private set; }
        public Thing Source { get; private set; }

        public ProductionEntry(ThingDef product, Thing source)
        {
            this.Product = product;
            this.Source = source;
        }
    }
}
