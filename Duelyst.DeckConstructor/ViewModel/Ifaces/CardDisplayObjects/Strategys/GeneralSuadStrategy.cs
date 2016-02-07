using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duelyst.DeckConstructor.CardCatalog;
using Duelyst.DeckConstructor.ViewModel.DeckCardItem;

namespace Duelyst.DeckConstructor.ViewModel.Ifaces.CardDisplayObjects.Strategys
{
    public class GeneralSuadStrategy : IDisplayStrategy
    {
        private CardGeneral _general;

        public GeneralSuadStrategy(CardGeneral general)
        {
            _general = general;
        }

        public IEnumerable<IDisplayableFilter> GetStrategyFilters()
        {
            var generals = Catalog.Instance.ViewModelGenerals.Where(g => g.IsNetural || g.Equals(_general));
            return generals;
        }
    }
}
