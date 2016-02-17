using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Duelyst.DeckConstructor.ViewModel;
using Duelyst.DeckConstructor.ViewModel.Communication;
using Duelyst.DeckConstructor.ViewModel.DeckCardItem;

namespace Duelyst.DeckConstructor.CardCatalog.Squad
{
    [Serializable]
    public class Squad : ListItemViewModelBase
    {
        public Squad()
        {
            SquadErrors = new List<SquadBuildError>();
            CardCountingCollection = new Dictionary<string, int>();
            SquadCardsList = new ObservableCollection<ListItemViewModelBase>();
        }

        [XmlIgnore]
        public int CardsInSquad
        {
            get { return SquadCardsList.Count; }
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
                return Name;
            }
            set
            {
                Name = value;
            }
        }

        /// <summary>
        /// Идентификаторы карт отряда и их количество
        /// </summary>
        [XmlElement]
        [DataMember]
        public KeyValuePair<string, int>[] CardSquadCountData
        {
            get { return CardCountingCollection.Select(p => p).ToArray(); }
            set
            {
                CardCountingCollection = value.ToDictionary(p => p.Key, p => p.Value);
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
        public Dictionary<string, int> CardCountingCollection { get; set; }

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
            if (card != null && CardCountingCollection.ContainsKey(card.CardId) && !card.Equals(SquadOwner))
            {
                var cardInSquad = CardCountingCollection[card.CardId] - 1;
                if (cardInSquad == 0)
                {
                    CardCountingCollection.Remove(card.CardId);
                }
                else
                {
                    CardCountingCollection[card.CardId] = cardInSquad;
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
        /// <param name="resp"></param>
        public bool TryAddCard(CardItemViewModelBase card, out CardAddResponse resp)
        {
            resp = new CardAddResponse();
            if (card.Owner != null && (!card.Owner.Equals(SquadOwner) && !card.Owner.IsNetural))
            {
                resp.ResponseType = EResponseType.OwnerError;
                return false;
            }
            else if (card.Owner == null)
            {
                resp.ResponseType = EResponseType.OwnerError;
                //TODO: PROCESS
            }

            if (CardsInSquad == SquadManager.MaxCardCount)
            {
                resp.ResponseType = EResponseType.SquadLimit;
                return false;
            }
            else if(CardsInSquad > SquadManager.MaxCardCount)
            {
                MarkSquadBroken("В отряде больше карт чем допустимо в игре");
            }

            int squadCount;
            if (CardCountingCollection.TryGetValue(card.CardId, out squadCount))
            {
                if (squadCount == card.MaxInDeck)
                {
                    resp.ResponseType = EResponseType.CardInstanceLimit;
                    return false;
                }

                if (squadCount > card.MaxInDeck)
                {
                    //Пометить отряд как сломанный
                    MarkSquadBroken(String.Format("В отряде {0} добавлено карт с ID {1} - {2} хотя допустимо {3}",
                    SquadName, card.CardId, squadCount, card.MaxInDeck));
                }

                CardCountingCollection[card.CardId] = squadCount + 1;
            }
            else
            {
                CardCountingCollection[card.CardId] = 1;
            }

            AddCardInorder(card);
            return true;
        }

        private void MarkSquadBroken(string msg)
        {
            IsBroken = true;
            SquadErrors.Add(new SquadBuildError(msg));
        }
    }
}
