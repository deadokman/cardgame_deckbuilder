using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            CardSquadCount = new Dictionary<string, int>();
            SquadCardsList = new ObservableCollection<ListItemViewModelBase>();
        }

        /// <summary>
        /// Имя отряда
        /// </summary>
        [XmlAttribute]
        [DataMember]
        public string SquadName
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }

        /// <summary>
        /// Идентификаторы карт отряда и их количество
        /// </summary>
        [XmlElement]
        [DataMember]
        public KeyValuePair<string, int>[] CardSquadCountData
        {
            get { return CardSquadCount.Select(p => p).ToArray(); }
            set
            {
                CardSquadCount = value.ToDictionary(p => p.Key, p => p.Value);
            }
        }

        [XmlElement]
        [DataMember]
        public string CardGeneralId { get; set; }

        /// <summary>
        /// Флаг того, что данный отряд сломан
        /// </summary>
        [XmlIgnore]
        public bool IsBroken { get; private set; }

        [XmlIgnore]
        public Dictionary<string, int> CardSquadCount { get; set; }

        /// <summary>
        /// Ошибки связанные с потстроением отряда
        /// </summary>
        [XmlIgnore]
        public List<SquadBuildError> SquadErrors { get; private set; } 

        /// <summary>
        /// Владелец отряда
        /// </summary>
        [XmlIgnore]
        public CardGeneral SquadOwner { get; set; }

        /// <summary>
        /// Карты входяие в отряд
        /// </summary>
        [XmlIgnore]
        public ObservableCollection<ListItemViewModelBase> SquadCardsList { get; set; }

        public CardItemViewModelBase[] SquadCards
        {
            get
            {
                return SquadCardsList.Cast<CardItemViewModelBase>().ToArray();
            }
        }

        public bool TryRemoveCard(CardItemViewModelBase card)
        {
            if (card != null && CardSquadCount.ContainsKey(card.CardId) && !card.Equals(SquadOwner))
            {
                var cardInSquad = CardSquadCount[card.CardId] - 1;
                if (cardInSquad == 0)
                {
                    CardSquadCount.Remove(card.CardId);
                }
                else
                {
                    CardSquadCount[card.CardId] = cardInSquad;
                }

                SquadCardsList.Remove(card);
                return true;
            }

            return false;
        }

        private void AddCardInorder(ListItemViewModelBase card)
        {
            var squadCard = card as CardItemViewModelBase;
            if (squadCard == null)
            {
                throw new ArgumentException("Аргумент должен иметь тип карты отряда");
            }

            var prevp = SquadCardsList.Where(i => i.ManaCost <= card.ManaCost).FirstOrDefault(i => String.Compare(i.Name, card.Name, StringComparison.InvariantCultureIgnoreCase) < 0);
            if (prevp != null)
            {
                var idx = SquadCardsList.IndexOf(prevp);
                SquadCardsList.Insert(idx + 1, squadCard);
            }
            else
            {
                SquadCardsList.Add(squadCard);
            }
        }

        /// <summary>
        /// Добавить карту к отряду
        /// </summary>
        /// <param name="card">Новая карта</param>
        public bool TryAddCard(CardItemViewModelBase card)
        {
            if (card.Owner != null && (!card.Owner.Equals(SquadOwner) && !card.Owner.IsNetural))
            {
                return false;
            }
            else if (card.Owner == null)
            {
                //TODO: PROCESS
            }

            int squadCount;
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
            }
            else
            {
                CardSquadCount[card.CardId] = 1;
            }

            AddCardInorder(card);
            return true;
        }


    }
}
