using System.Collections.Generic;

namespace Duelyst.DeckConstructor.ViewModel.Ifaces.CardDisplayObjects
{
    public interface IDisplayStrategy
    {
        IEnumerable<IDisplayableFilter> GetStrategyFilters();
    }
}
