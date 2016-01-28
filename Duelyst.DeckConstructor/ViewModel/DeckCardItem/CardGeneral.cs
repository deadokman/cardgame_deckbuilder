using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;

namespace Duelyst.DeckConstructor.ViewModel.DeckCardItem
{
    public class CardGeneral : CardItemViewModelBase, IEquatable<CardGeneral>
    {
        private bool _isAvailebleToSelect;
        private bool _isSelected;

        public CardGeneral(string name)
            : base(name)
        {
            CardViewModels = new List<CardItemViewModelBase>();
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

        /// <summary>
        /// Cписок подчиненных генералу карт
        /// </summary>
        public List<CardItemViewModelBase> CardViewModels { get; set; }

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
