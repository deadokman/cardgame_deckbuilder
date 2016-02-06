using System;
using System.Collections.Generic;
using System.Linq;
using Duelyst.DeckConstructor.ViewModel.Ifaces.CardDisplayObjects;

namespace Duelyst.DeckConstructor.ViewModel.DeckCardItem
{
    public class CardGeneral : CardItemViewModelBase, IDisplayableFilter, IEquatable<CardGeneral>
    {
        private bool _isAvailebleToSelect;
        private bool _isSelected;

        public CardGeneral(string name)
            : base(name)
        {
            CardViewModelsDictionary = new Dictionary<string, CardItemViewModelBase>();
            IsAvailebleToSelect = true;
        }

        public event FilterSelectionChanged Selected;

        private void RaiseIamSelected()
        {
            if (Selected != null)
            {
                Selected(this);
            }
        }

        public void AddCard(CardItemViewModelBase item)
        {
            if (CardViewModelsDictionary.ContainsKey(item.CardId))
            {
                throw new ArgumentException(
                    $"Карта с идентфикатором {item.CardId} уже добавлена к списку подчиненных генералу");
            }

            item.Owner = this;
            CardViewModelsDictionary.Add(item.CardId, item);
        }

        public IList<IDisplayadble> ChildData
        {
            get
            {
                return CardViewModelsDictionary.Select(i => i.Value as IDisplayadble).ToList();
            }
        }

        public CardItemViewModelBase[] CardViewModels
        {
            get { return CardViewModelsDictionary.Select(pair => pair.Value).ToArray(); }
        }

        /// <summary>
        /// Cписок подчиненных генералу карт
        /// </summary>
        private Dictionary<string, CardItemViewModelBase> CardViewModelsDictionary { get; set; }

        /// <summary>
        /// Генерал доступен для выбора
        /// </summary>
        public bool IsAvailebleToSelect
        {
            get { return _isAvailebleToSelect; }
            set
            {
                _isAvailebleToSelect = value;
                RaisePropertyChanged(() => IsAvailebleToSelect);
            }
        }

        /// <summary>
        /// Генерал выбран
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value; 
                RaisePropertyChanged(() => IsSelected);
                if (_isSelected)
                {
                    RaiseIamSelected();
                }
            }
        }

        public bool Equals(CardGeneral other)
        {
            return other!= null && this.Name == other.Name;
        }

    }
}
