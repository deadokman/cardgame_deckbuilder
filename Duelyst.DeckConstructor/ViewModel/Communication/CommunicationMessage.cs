using System.Drawing;

using Duelyst.DeckConstructor.CardCatalog.Squad;

namespace Duelyst.DeckConstructor.ViewModel.Communication
{
    public  class CommunicationMessage
    {
        public CommunicationMessage(CommEventType type)
        {
            CommMsgType = type;
        }

        public CommEventType CommMsgType { get; protected set; }

        public Squad SquadToDisplay { get; set; }

        public CardAddResponse CardAddResponse { get; set; }
    }
}
