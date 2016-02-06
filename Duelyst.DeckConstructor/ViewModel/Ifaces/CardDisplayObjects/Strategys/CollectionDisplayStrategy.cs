using System.Collections.Generic;
using Duelyst.DeckConstructor.CardCatalog;

namespace Duelyst.DeckConstructor.ViewModel.Ifaces.CardDisplayObjects.Strategys
{
    public class CollectionDisplayStrategy : IDisplayStrategy
    {
        public IEnumerable<IDisplayableFilter> GetStrategyFilters()
        {
            return Catalog.Instance.ViewModelGenerals;
        }
    }
}
