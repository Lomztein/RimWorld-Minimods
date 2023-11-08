using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Lomzie.ProducedLocallyFilter.ProductionSources
{
    public class ProductionSource_WorkTable : ProductionSource
    {
        public override IEnumerable<ProductionEntry> GetProducedOnMap(Map map)
        {
            var workTables = map.listerBuildings.AllBuildingsColonistOfClass<Building_WorkTable>();
            foreach (var workTable in workTables)
            {
                foreach (var bill in workTable.BillStack)
                {
                    if (bill.recipe.products != null)
                    {
                        foreach (var  product in bill.recipe.products)
                        {
                            yield return new ProductionEntry(product.thingDef, workTable);
                        }
                    }
                }
            }
        }
    }
}
