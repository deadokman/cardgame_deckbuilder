using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Duelyst.DeckConstructor.CardCatalog.Squad
{
    [Serializable]
    public class SquadDto
    {
        [XmlAttribute]
        [DataMember]
        public string SquadName { get; set; }

        [XmlArray]
        [DataMember]
        public List<DtoCardCountInfo> CardCountInfo { get; set; }

        [XmlAttribute]
        [DataMember]
        public string GeneralId { get; set; }

        public Squad GetSquad()
        {
            return  new Squad { CardGeneralId = GeneralId, SquadName = SquadName,
                CardCountingCollection =  CardCountInfo.ToDictionary(i => i.CardId, i => i.Count)};
        }
    }
}
