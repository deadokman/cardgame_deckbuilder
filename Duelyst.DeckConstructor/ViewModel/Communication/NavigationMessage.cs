using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duelyst.DeckConstructor.ViewModel.Communication
{
    public  class NavigationMessage
    {
        public NavigationMessage(CommEventType type)
        {
            CommMsgType = type;
        }
        public CommEventType CommMsgType { get; protected set; }
    }
}
