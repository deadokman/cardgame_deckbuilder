using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using Duelyst.DeckConstructor.ViewModel.DeckCardItem;

namespace Duelyst.DeckConstructor.CardCatalog.Squad
{
    [Serializable]
    public class Squad
    {
        public Squad()
        {
            SquadErrors = new List<SquadBuildError>();
            _squadCards = new List<CardItemViewModelBase>();
        }
        /// <summary>
        /// Имя отряда
        /// </summary>
        [XmlAttribute]
        public string SquadName { get; set; }

        /// <summary>
        /// Идентификаторы карт отряда и их количество
        /// </summary>
        [XmlElement]
        public Dictionary<string, int> CardSquadCount { get; set; }

        /// <summary>
        /// Флаг того, что данный отряд сломан
        /// </summary>
        public bool IsBroken { get; private set; }

        /// <summary>
        /// Ошибки связанные с потстроением отряда
        /// </summary>
        public List<SquadBuildError> SquadErrors { get; private set; } 

        /// <summary>
        /// Карты входяие в отряд
        /// </summary>
        public CardItemViewModelBase[] SquadCards { get; private set; }

        private List<CardItemViewModelBase> _squadCards; 
 
        public void InitCards(Func<string, CardItemViewModelBase> initCatdDelegate)
        {
            foreach (var cardPair in CardSquadCount)
            {
                var cardItem = initCatdDelegate(cardPair.Key);
                if (cardItem == null)
                {
                    IsBroken = true;
                    SquadErrors.Add(new SquadBuildError($"Не удалось найти карту с айди {cardPair.Key} в каталоге"));
                    continue;
                }

                _squadCards.Add(cardItem);
            }

            SquadCards = _squadCards.ToArray();
        }


    }
}
