using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Duelyst.DeckConstructor.CardCatalog.Squad;
using Duelyst.DeckConstructor.ViewModel.Communication;
using Duelyst.DeckConstructor.ViewModel.DeckCardItem;
using Duelyst.DeckConstructor.ViewModel.Ifaces.CardDisplayObjects;
using Duelyst.DeckConstructor.ViewModel.Ifaces.CardDisplayObjects.Strategys;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Duelyst.DeckConstructor.ViewModel
{
    public class SquadManagerViewModel : ViewModelBase
    {
        private Dictionary<ESquadBuilderModeType, IDisplayStrategy> Strategys; 


        public SquadManagerViewModel()
        {
            CardListItems = new ObservableCollection<ListItemViewModelBase>();
            CardCollectionMode = true;
            NewSquadCommand = new RelayCommand(EnterSquadBuilderMode);
            MakePicture = new RelayCommand(OnMakePicture);
            MessengerInstance.Register(this, (CardClickMessage m) => ReciveCardClick(m));
            _squadManager = SquadManager.Instance;
            Strategys = new Dictionary<ESquadBuilderModeType, IDisplayStrategy>();
            Strategys.Add(ESquadBuilderModeType.CollectionMode, new CollectionDisplayStrategy());
            Strategys.Add(ESquadBuilderModeType.GeneralSelectMode, new GeneralSelectStrategy());
            _currentBuildMode = ESquadBuilderModeType.CollectionMode;
            MessengerInstance.Send(Strategys[_currentBuildMode]);
        }

        private SquadManager _squadManager;

        /// <summary>
        /// Текущий создаваемый отряд
        /// </summary>
        private Squad _сurrentBuildingSquad;

        public string CurrentSquadName
        {
            get { return _сurrentBuildingSquad == null ? "КОЛЛЕКЦИЯ" : _сurrentBuildingSquad.Name; }
            set
            {
                _сurrentBuildingSquad.Name = value;
                RaisePropertyChanged(() => CurrentSquadName);
            }
        }

        public ObservableCollection<ListItemViewModelBase> CardListItems
        {
            get { return _cardListItems; }
            set
            {
                _cardListItems = value; 
                RaisePropertyChanged(() => CardListItems);
            }
        }

        /// <summary>
        /// Команда переводящая вью модель в режим построение отряда
        /// </summary>
        public ICommand NewSquadCommand { get; set; }

        /// <summary>
        /// Сделать изображение отряда
        /// </summary>
        public ICommand MakePicture { get; set; }

        private ESquadBuilderModeType _currentBuildMode;


        /// <summary>
        /// Выбранная карта
        /// </summary>
        public CardItemViewModelBase SelectedCardItem
        {
            get { return _selectedCardItem; }
            set
            {
                _selectedCardItem = value;
                RemoveCard(value);

            }
        }

        private void RemoveCard(ListItemViewModelBase card)
        {
            if (card != null)
            {
                _сurrentBuildingSquad.TryRemoveCard(card);
            }
        }

        private void EnterSquadBuilderMode()
        {
            CardCollectionMode = false;
            MessengerInstance.Send(Strategys[ESquadBuilderModeType.GeneralSelectMode]);
        }

        public void ReciveCardClick(CardClickMessage message)
        {
            var card = message.Card;
            if (!CardCollectionMode)
            {
                //Если в режиме сбора отряда
                //TODO:Произвести проверку возможности добавления карты в текущий отряд, выполнить добавление
                var general = card as CardGeneral;
                if (general != null)
                {
                    _сurrentBuildingSquad = _squadManager.InitNewSquad(general);
                    CardListItems = _сurrentBuildingSquad.SquadCardsList;
                    _currentBuildMode = ESquadBuilderModeType.SquadBuildMode;
                    //Необходимость действий со справочником сомнительна. Сделано для единообразия
                    Strategys[_currentBuildMode] = new GeneralSuadStrategy(general);
                    _сurrentBuildingSquad.SquadCardsList.Add(general);
                    CurrentSquadName = String.Format("Отряд {0}", general.Name);
                    MessengerInstance.Send(Strategys[_currentBuildMode]);
                    return;
                }

                if (_сurrentBuildingSquad == null)
                {
                    throw new Exception("Не создан экземпляр нового отряда");
                }

                if (!_сurrentBuildingSquad.TryAddCard(card))
                {
                 
                }
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

        private void OnMakePicture()
        {
            if (_сurrentBuildingSquad != null)
            {
                var image =ToPictureProcessor.SquadToImage(_сurrentBuildingSquad);

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
        private CardItemViewModelBase _selectedCardItem;
        private ObservableCollection<ListItemViewModelBase> _cardListItems;
    }
}
