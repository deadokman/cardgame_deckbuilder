using Duelyst.DeckConstructor.ViewModel.DeckCardItem;

namespace Duelyst.DeckConstructor.ViewModel.Communication
{
    /// <summary>
    /// Сообщение о клике на карту
    /// </summary>
    public class CardClickMessage
    {
        public CardItemViewModelBase Card { get; private set; }

        public CardClickMessage(CardItemViewModelBase vm)
        {
            Card = vm;
        }
    }
}
