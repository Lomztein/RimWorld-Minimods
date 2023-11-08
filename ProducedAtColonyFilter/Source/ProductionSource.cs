using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Lomzie.ProducedLocallyFilter
{
    public abstract class ProductionSource
    {
        public abstract IEnumerable<ProductionEntry> GetProducedOnMap(Map map);
    }
}
