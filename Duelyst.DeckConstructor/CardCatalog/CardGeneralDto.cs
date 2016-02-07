using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Duelyst.DeckConstructor.CardCatalog
{
    [Serializable]
    public class CardGeneralDto : CardDtoItem
    {
        [XmlAttribute]
        public string GeneralEmblemName { get; set; }

        [XmlArray("GeneralCards")]
        public List<CardDtoItem> Cards { get; set; }
    }
}
