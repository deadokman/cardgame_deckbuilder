using System.Collections.Generic;
using System.Linq;
using Duelyst.DeckConstructor.CardCatalog;
using Duelyst.DeckConstructor.ViewModel.Communication;

namespace Duelyst.DeckConstructor.ViewModel.Ifaces.CardDisplayObjects.Strategys
{
    public class GeneralSelectStrategy : IDisplayStrategy
    {

        public IEnumerable<IDisplayableFilter> GetStrategyFilters()
        {
            var strat = new CustomDisplayFilter(Catalog.Instance.Generals.Where(g => !g.IsNetural).Cast<IDisplayadble>().ToList());
            return new[] {strat};
        }
    }
}
