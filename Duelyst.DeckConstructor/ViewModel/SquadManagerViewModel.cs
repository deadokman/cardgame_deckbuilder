using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Duelyst.DeckConstructor.CardCatalog.Squad;
using Duelyst.DeckConstructor.ViewModel.Communication;
using Duelyst.DeckConstructor.ViewModel.DeckCardItem;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Duelyst.DeckConstructor.ViewModel
{
    public class SquadManagerViewModel : ViewModelBase
    {
        public SquadManagerViewModel()
        {
            CardListItems = new ObservableCollection<ListItemViewModelBase>();
            CardCollectionMode = true;
            NewSquadCommand = new RelayCommand(EnterSquadBuilderMode);
            MessengerInstance.Register(this, (CardClickMessage m) => ReciveCardClick(m));
            _squadManager = SquadManager.Instance;
        }

        private SquadManager _squadManager;

        /// <summary>
        /// Текущий создаваемый отряд
        /// </summary>
        private Squad _сurrentBuildingSquad;

        public ObservableCollection<ListItemViewModelBase> CardListItems { get; set; }

        /// <summary>
        /// Команда переводящая вью модель в режим построение отряда
        /// </summary>
        public ICommand NewSquadCommand { get; set; }

        private void EnterSquadBuilderMode()
        {
            CardCollectionMode = false;
        }

        public void ReciveCardClick(CardClickMessage message)
        {
            var card = message.Card;
            if (!CardCollectionMode)
            {
                //Если в режиме сбора отряда
                //TODO:Произвести проверку возможности добавления карты в текущий отряд, выполнить добавление
                CardListItems.Add(message.Card);
                var general = card as CardGeneral;
                if (general != null)
                {
                    _сurrentBuildingSquad = _squadManager.InitNewSquad(general);
                }

                if (_сurrentBuildingSquad == null)
                {
                    throw new Exception("Не создан экземпляр нового оряда");
                }

                _сurrentBuildingSquad.TryAddCard(card);
            }
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
