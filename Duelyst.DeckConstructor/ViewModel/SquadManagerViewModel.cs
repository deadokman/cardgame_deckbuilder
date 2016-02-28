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
using static System.String;

namespace Duelyst.DeckConstructor.ViewModel
{
    public class SquadManagerViewModel : ViewModelBase
    {
        private Dictionary<ESquadBuilderModeType, IDisplayStrategy> Strategys; 

        public SquadManagerViewModel()
        {
            _squadManager = SquadManager.Instance;
            CardListItems = new ObservableCollection<ListItemViewModelBase>();
            CardCollectionMode = true;
            NewSquadCommand = new RelayCommand(EnterSquadBuilderMode);
            MakePicture = new RelayCommand(OnMakePicture);
            SaveSquadCmd = new RelayCommand(OnSaveSquadClick);
            MessengerInstance.Register(this, (CardClickMessage m) => ReciveCardClick(m));
            Strategys = new Dictionary<ESquadBuilderModeType, IDisplayStrategy>();
            Strategys.Add(ESquadBuilderModeType.CollectionMode, new CollectionDisplayStrategy());
            Strategys.Add(ESquadBuilderModeType.GeneralSelectMode, new GeneralSelectStrategy());
            CardItemClickCmd = new RelayCommand<CardItemViewModelBase>(OnCardItemClick);
            MessengerInstance.Send(Strategys[ESquadBuilderModeType.CollectionMode]);
        }

        private void OnSaveSquadClick()
        {
            if (_сurrentBuildingSquad.CardsInSquad == SquadManager.MaxCardCount)
            {
                SquadManager.Instance.StoreSquadToDefaultLocation(_сurrentBuildingSquad);
                CardCollectionMode = true;
                CardListItems = new ObservableCollection<ListItemViewModelBase>(_squadManager.Squads);
                MessengerInstance.Send(Strategys[ESquadBuilderModeType.CollectionMode]);
            }
            else
            {
                var commMsg = new CommunicationMessage(CommEventType.CardAddResponseMessage);
                commMsg.CardAddResponse = new CardAddResponse
                {
                    ResponseType = EResponseType.SquadSaveUnavaileble
                };
                Messenger.Default.Send(commMsg);
            }
        }

        private void OnCardItemClick(ListItemViewModelBase listItem)
        {
            if (SquadBuilderMode)
            {
                var cardItemVm = listItem as CardItemViewModelBase;
                if (_сurrentBuildingSquad != null)
                {
                    _сurrentBuildingSquad.TryRemoveCard(cardItemVm);
                }
            }
            else
            {
                var squad = listItem as Squad;
                if (squad != null)
                {
                    _сurrentBuildingSquad = squad;
                    InitNewSquadEnvirement(squad);
                }
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
                    ? Format(str, 0, SquadManager.MaxCardCount)
                    : Format(str, _сurrentBuildingSquad.CardsInSquad, SquadManager.MaxCardCount);
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

        /// <summary>
        /// Обработчик клика на элемент в списке
        /// </summary>
        public ICommand CardItemClickCmd { get; set; }

        /// <summary>
        /// Обработка нажатия на кнопку сохранения отряда
        /// </summary>
        public ICommand SaveSquadCmd { get; set; }

        private void EnterSquadBuilderMode()
        {
            CardCollectionMode = false;
            MessengerInstance.Send(Strategys[ESquadBuilderModeType.GeneralSelectMode]);
        }

        private void InitNewSquadEnvirement(Squad squad)
        {
            _сurrentBuildingSquad = squad;
            CardListItems = _сurrentBuildingSquad.SquadCardsList;
            //Необходимость действий со справочником сомнительна. Сделано для единообразия
            Strategys[ESquadBuilderModeType.SquadBuildMode] = new GeneralSuadStrategy(squad.SquadOwner);
            MessengerInstance.Send(Strategys[ESquadBuilderModeType.SquadBuildMode]);
            CardCollectionMode = false;
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
                    var squad = _squadManager.InitNewSquad(general);
                    squad.SquadName = Format("Отряд {0}", general.Name);
                    squad.SquadCardsList.Add(general);
                    InitNewSquadEnvirement(squad);
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
