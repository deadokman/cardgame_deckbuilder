using System.Drawing;

using Duelyst.DeckConstructor.CardCatalog.Squad;

namespace Duelyst.DeckConstructor.ViewModel.Communication
{
    public  class NavigationMessage
    {
        public NavigationMessage(CommEventType type)
        {
            CommMsgType = type;
        }

        public CommEventType CommMsgType { get; protected set; }

        public Squad SquadToDisplay { get; set; }
    }
}
