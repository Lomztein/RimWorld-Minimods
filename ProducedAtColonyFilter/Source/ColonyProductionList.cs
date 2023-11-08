using Lomzie.ProducedLocallyFilter;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Lomzie.ProducedLocallyFilter
{
    public class ColonyProductionList
    {
        public Dictionary<ThingDef, List<ProductionEntry>> LocalProduction = new Dictionary<ThingDef, List<ProductionEntry>>();

        public bool IsProducedLocally(ThingDef def)
            => LocalProduction.ContainsKey(def);

        public IEnumerable<Thing> GetProductionSources(ThingDef def)
        {
            if (IsProducedLocally(def))
            {
                return LocalProduction[def].Select(x => x.Source);
            }
            return Enumerable.Empty<Thing>();
        }

        public void Clear()
        {
            LocalProduction.Clear();
        }

        public void Add(ThingDef def, ProductionEntry entry)
        {
            if (!LocalProduction.ContainsKey(def))
            {
                LocalProduction.Add(def, new List<ProductionEntry>());
            }
            LocalProduction[def].Add(entry);
        }
    }
}