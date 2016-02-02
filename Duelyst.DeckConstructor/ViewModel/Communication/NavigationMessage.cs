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
