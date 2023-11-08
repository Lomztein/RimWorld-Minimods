using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Lomzie.ProducedLocallyFilter
{
    public class SpecialThingFilterWorker_ProducedLocally : SpecialThingFilterWorker
    {
        public override bool Matches(Thing t)
        {
            Map map = t.Map ?? t.MapHeld;
            if (map == null) return false;
            return ProductionTracker.Instance?.IsProducedLocally(map, t.def) ?? false;
        }

        public override bool AlwaysMatches(ThingDef def)
        {
            return false;
        }

        public override bool CanEverMatch(ThingDef def)
        {
            return true;
        }
    }
}
