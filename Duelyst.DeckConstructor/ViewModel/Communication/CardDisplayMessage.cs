using Duelyst.DeckConstructor.ViewModel.DeckCardItem;
using Duelyst.DeckConstructor.ViewModel.Ifaces.CardDisplayObjects;

namespace Duelyst.DeckConstructor.ViewModel.Communication
{
    public class CardDisplayMessage
    {
        public IDisplayStrategy Strategy { get; set; }

        public CardDisplayMessage(IDisplayStrategy strategy)
        {
            Strategy = strategy;
        }
    }
}
