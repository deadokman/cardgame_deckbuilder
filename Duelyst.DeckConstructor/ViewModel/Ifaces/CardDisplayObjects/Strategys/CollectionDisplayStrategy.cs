using System.Collections.Generic;
using Duelyst.DeckConstructor.CardCatalog;

namespace Duelyst.DeckConstructor.ViewModel.Ifaces.CardDisplayObjects.Strategys
{
    public class CollectionDisplayStrategy : IDisplayStrategy
    {
        public IEnumerable<IDisplayableFilter> GetStrategyFilters()
        {
            var generals = Catalog.Instance.ViewModelGenerals;
            foreach (var cardGeneral in generals)
            {
                cardGeneral.IsAvailebleToSelect = true;
            }
            return generals;
        }
    }
}
