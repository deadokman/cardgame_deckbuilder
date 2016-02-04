using Duelyst.DeckConstructor.ViewModel.DeckCardItem;

namespace Duelyst.DeckConstructor.ViewModel.Communication
{
    public class CardDisplayMessage
    {
        public SquadBuilderModeType ModeType { get; set; }

        public CardGeneral FilterGeneral { get; set; }

        public CardDisplayMessage(SquadBuilderModeType modeType)
        {
            ModeType = modeType;
        }
    }
}
