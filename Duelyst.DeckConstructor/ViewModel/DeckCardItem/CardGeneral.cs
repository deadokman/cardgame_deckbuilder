using GalaSoft.MvvmLight;

namespace Duelyst.DeckConstructor.ViewModel.DeckCardItem
{
    public class CardGeneral : ViewModelBase
    {
        private bool _isAvailebleToSelect;
        private bool _isSelected;

        public CardGeneral(string generalName)
        {
            GeneralName = generalName;
        }

        public string GeneralName { get; set; }

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
