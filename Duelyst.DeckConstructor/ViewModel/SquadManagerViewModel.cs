using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Duelyst.DeckConstructor.CardCatalog.Squad;
using Duelyst.DeckConstructor.Pages;
using Duelyst.DeckConstructor.ViewModel.Communication;
using Duelyst.DeckConstructor.ViewModel.DeckCardItem;
using Duelyst.DeckConstructor.ViewModel.Ifaces.CardDisplayObjects;
using Duelyst.DeckConstructor.ViewModel.Ifaces.CardDisplayObjects.Strategys;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

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
            CardItemClickCmd = new RelayCommand<CardItemViewModelBase>(OnCardItemClick);
            MessengerInstance.Send(Strategys[_currentBuildMode]);
        }

        private void OnCardItemClick(CardItemViewModelBase cardItemViewModelBase)
        {
            if (_сurrentBuildingSquad != null)
            {
                _сurrentBuildingSquad.TryRemoveCard(cardItemViewModelBase);
            }
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

        public string CardInDeck
        {
            get
            {
                var str = "{0}\\{1}";
                return _сurrentBuildingSquad == null
                    ? String.Format(str, 0, SquadManager.MaxCardCount)
                    : String.Format(str, _сurrentBuildingSquad.CardsInSquad, SquadManager.MaxCardCount);
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
        /// Обработчик клика на элемент в списке
        /// </summary>
        public ICommand CardItemClickCmd { get; set; }

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
                PersistButtonAvaileble = false;
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

                CardAddResponse resp;
                if (!_сurrentBuildingSquad.TryAddCard(card, out resp))
                {
                    var commMsg = new CommunicationMessage(CommEventType.CardAddResponseMessage);
                    commMsg.CardAddResponse = resp;
                    Messenger.Default.Send(commMsg);
                }
                else
                {
                    RaisePropertyChanged(() => CardInDeck);
                    PersistButtonAvaileble = _сurrentBuildingSquad.CardsInSquad == SquadManager.MaxCardCount;
                }
            }
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
                var msg = new CommunicationMessage(CommEventType.WindowPreview);
                msg.SquadToDisplay = _сurrentBuildingSquad;
                var w = new GeneratedImagePreview();
                w.Show();
                Messenger.Default.Send(msg);
                //
            }
        }

        /// <summary>
        /// Доступность кнопки сохранения отряда
        /// </summary>
        public bool PersistButtonAvaileble
        {
            get { return _persistButtonAvaileble; }
            set
            {
                _persistButtonAvaileble = value; 
                RaisePropertyChanged(() => PersistButtonAvaileble);
            }
        }

        /// <summary>
        /// Режим редактирования коллекции карт
        /// </summary>
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
        private ObservableCollection<ListItemViewModelBase> _cardListItems;
        private bool _persistButtonAvaileble;
    }
}
