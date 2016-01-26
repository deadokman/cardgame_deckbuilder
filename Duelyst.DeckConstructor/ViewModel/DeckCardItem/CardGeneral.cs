using GalaSoft.MvvmLight;

namespace Duelyst.DeckConstructor.ViewModel.DeckCardItem
{
    public class CardGeneral : DeckCardItemViewModel
    {
        private bool _isAvailebleToSelect;
        private bool _isSelected;

        public CardGeneral(string name)
            :base(0, name)
        {
        }


        public bool IsAvailebleToSelect
        {
            get { return _isAvailebleToSelect; }
            set
            {
                _isAvailebleToSelect = value;
                RaisePropertyChanged(() => IsAvailebleToSelect);
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value; 
                RaisePropertyChanged(() => IsSelected);
            }
        }
    }
}
