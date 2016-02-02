using System;
using System.Collections.Generic;
using System.Linq;

namespace Duelyst.DeckConstructor.ViewModel.DeckCardItem
{
    public class CardGeneral : CardItemViewModelBase, IEquatable<CardGeneral>
    {
        private bool _isAvailebleToSelect;
        private bool _isSelected;

        public CardGeneral(string name)
            : base(name)
        {
            CardViewModelsDictionary = new Dictionary<string, CardItemViewModelBase>();
        }

        public delegate void GeneralSelectionChanged(CardGeneral general);

        public event GeneralSelectionChanged IAmSelected;

        private void RaiseIamSelected()
        {
            if (IAmSelected != null)
            {
                IAmSelected(this);
            }
        }

        public void AddCard(CardItemViewModelBase item)
        {
            if (CardViewModelsDictionary.ContainsKey(item.CardId))
            {
                throw new ArgumentException(
                    $"Карта с идентфикатором {item.CardId} уже добавлена к списку подчиненных генералу");
            }

            CardViewModelsDictionary.Add(item.CardId, item);
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
