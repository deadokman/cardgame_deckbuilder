using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duelyst.DeckConstructor.ViewModel.DeckCardItem;
using GalaSoft.MvvmLight;

namespace Duelyst.DeckConstructor.ViewModel
{
    public class SquadManagerViewModel : ViewModelBase
    {
        public SquadManagerViewModel()
        {
            CardListItems = new ObservableCollection<CardListItemViewModelBase>();
        }

        public ObservableCollection<CardListItemViewModelBase> CardListItems { get; set; } 

        public bool CardCollectionMode
        {
            get
            {
                return _cardCollectionObserverMod;

            }
            set
            {
                _cardCollectionObserverMod = value;
                RaisePropertyChanged(() => CardCollectionMode);
            }
        }
        private bool _cardCollectionObserverMod;
    }
}
