using Lomzie.ProducedLocallyFilter;
using Lomzie.ProducedLocallyFilter.ProductionSources;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Lomzie.ProducedLocallyFilter
{
    public class ProductionTracker : WorldComponent
    {
        public static ProductionTracker Instance;

        public Dictionary<Map, ColonyProductionList> ProductionLists = new Dictionary<Map, ColonyProductionList>();
        public const int UpdateTickDelay = 6000;
        private int _tickIndex = 5900;

        public static List<ProductionSource> Sources = new List<ProductionSource>
        {
            new ProductionSource_WorkTable(),
        };

        public ProductionTracker(World world) : base(world)
        {
            Instance = this;
        }

        public override void WorldComponentTick()
        {
            if (Find.CurrentMap != null)
            {
                _tickIndex++;
                if (_tickIndex >= UpdateTickDelay)
                {
                    _tickIndex = 0;
                    UpdateProduction();

                    Log.Message("Locally produced on current map: " + string.Join(", ", ProductionLists[Find.CurrentMap].LocalProduction.Select(x => x.Key.defName)));
                }
            }
        }

        private void UpdateProduction()
        {
            var current = Find.CurrentMap;
            if (ProductionLists.TryGetValue(current, out var list))
            {
                list.Clear();
            }
            else
            {
                ProductionLists.Add(current, new ColonyProductionList());
            }
            foreach (var source in Sources)
            {
                var produced = source.GetProducedOnMap(current);
                foreach (var item in produced)
                {
                    ProductionLists[current].Add(item.Product, item);
                }
            }
        }

        public bool IsProducedLocally(Map map, ThingDef def)
        {
            if (ProductionLists.TryGetValue(map, out var list))
            {
                return list.IsProducedLocally(def);
            }
            return false;
        }

        public IEnumerable<Thing> GetProductionSources(Map map, ThingDef def)
        {
            if (ProductionLists.TryGetValue(map, out var list))
            {
                return list.GetProductionSources(def);
            }
            return Enumerable.Empty<Thing>();
        }
    }
}
