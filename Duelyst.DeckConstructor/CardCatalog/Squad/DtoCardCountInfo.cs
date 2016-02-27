using System;
using System.Xml.Serialization;

namespace Duelyst.DeckConstructor.CardCatalog.Squad
{
    [Serializable]
    public class DtoCardCountInfo
    {
        [XmlAttribute]
        public string CardId { get; set; }

        [XmlAttribute]
        public int Count { get; set; }

        public DtoCardCountInfo(string cardId, int count)
        {
            CardId = cardId;
            Count = count;
        }
    }
}
