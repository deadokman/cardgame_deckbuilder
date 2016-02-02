namespace Duelyst.DeckConstructor.CardCatalog.Squad
{
    public class SquadBuildError
    {
        public string Message { get; private set; }

        public string SquadDescriptorContent { get; set; }

        public SquadBuildError(string msg)
        {
            Message = msg;
        }
    }
}
