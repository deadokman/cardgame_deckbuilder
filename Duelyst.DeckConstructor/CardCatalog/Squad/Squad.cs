using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Duelyst.DeckConstructor.ViewModel.DeckCardItem;

namespace Duelyst.DeckConstructor.CardCatalog.Squad
{
    [Serializable]
    public class Squad : ListItemViewModelBase
    {
        public Squad()
        {
            SquadErrors = new List<SquadBuildError>();
            _squadCards = new Dictionary<string, CardItemViewModelBase>();
        }

        /// <summary>
        /// Имя отряда
        /// </summary>
        [XmlAttribute]
        [DataMember]
        public string SquadName { get; set; }

        /// <summary>
        /// Идентификаторы карт отряда и их количество
        /// </summary>
        [XmlElement]
        [DataMember]
        public Dictionary<string, int> CardSquadCount { get; set; }

        [XmlElement]
        [DataMember]
        public string CardGeneralId { get; set; }

        /// <summary>
        /// Флаг того, что данный отряд сломан
        /// </summary>
        public bool IsBroken { get; private set; }

        /// <summary>
        /// Ошибки связанные с потстроением отряда
        /// </summary>
        public List<SquadBuildError> SquadErrors { get; private set; } 

        /// <summary>
        /// Владелец отряда
        /// </summary>
        public CardGeneral SquadOwner { get; set; }

        /// <summary>
        /// Карты входяие в отряд
        /// </summary>
        public CardItemViewModelBase[] SquadCards
        {
            get
            {
                return _squadCards.Select(s => s.Value).ToArray();
            }
        }

        /// <summary>
        /// Список карт отряда
        /// </summary>
        private Dictionary<string, CardItemViewModelBase> _squadCards; 
        
        /// <summary>
        /// Добавить карту к отряду
        /// </summary>
        /// <param name="card">Новая карта</param>
        public bool TryAddCard(CardItemViewModelBase card)
        {

            if (card.Owner != null && !card.Owner.Equals(SquadOwner))
            {
                return false;
            }
            else if (card.Owner == null)
            {
                //TODO: PROCESS
            }

            int squadCount = 1;
            if (CardSquadCount.TryGetValue(card.CardId, out squadCount))
            {
                if (squadCount == card.MaxInDeck)
                {
                    return false;
                }

                if (squadCount > card.MaxInDeck)
                {
                    //Пометить отряд как сломанный
                    IsBroken = true;
                    SquadErrors.Add(new SquadBuildError(
                        String.Format("В отряде {0} добавлено карт с ID {1} - {2} хотя допустимо {3}", 
                            this.SquadName, card.CardId, squadCount, card.MaxInDeck)));
                    return false;
                }

                CardSquadCount[card.CardId] = squadCount + 1;
                return true;
            }

            CardSquadCount[card.CardId] = squadCount;
            _squadCards.Add(card.CardId, card);
            return true;
        }


    }
}
