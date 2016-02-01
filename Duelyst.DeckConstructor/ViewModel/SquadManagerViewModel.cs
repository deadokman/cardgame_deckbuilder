using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Duelyst.DeckConstructor.ViewModel.DeckCardItem;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Duelyst.DeckConstructor.ViewModel
{
    public class SquadManagerViewModel : ViewModelBase
    {
        public SquadManagerViewModel()
        {
            CardListItems = new ObservableCollection<CardListItemViewModelBase>();
            CardCollectionMode = true;
            NewSquadCommand = new RelayCommand(EnterSquadBuilderMode);
        }

        public ObservableCollection<CardListItemViewModelBase> CardListItems { get; set; }

        /// <summary>
        /// Команда переводящая вью модель в режим построение отряда
        /// </summary>
        public ICommand NewSquadCommand { get; set; }

        private void EnterSquadBuilderMode()
        {
            CardCollectionMode = false;

        }

        private void BuildSquadCollectionItems()
        {
            
        }

        private void TryAddCardToSquad()
        {
            
        }

        public bool SquadBuilderMode
        {
            get
            {
                return !_cardCollectionObserverMod;

            }
        }

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
                RaisePropertyChanged(() => SquadBuilderMode);
            }
        }
        private bool _cardCollectionObserverMod;
    }
}
