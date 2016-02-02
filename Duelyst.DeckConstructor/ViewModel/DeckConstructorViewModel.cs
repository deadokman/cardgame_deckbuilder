using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Duelyst.DeckConstructor.CardCatalog;
using Duelyst.DeckConstructor.ViewModel.DeckCardItem;
using GalaSoft.MvvmLight.CommandWpf;

namespace Duelyst.DeckConstructor.ViewModel
{
    public class DeckConstructorViewModel : ResizableViewModelBase
    {
        public DeckConstructorViewModel()
        {
            Catalog = CardCatalog.Catalog.Instance;
            InitCardChartInfoCollection();
            DeckCardItems = new ObservableCollection<CardItemViewModelBase>();
            Generals = new ObservableCollection<CardGeneral>(Catalog.ViewModelGenerals);
            DisplayedCardViewModels = new List<CardItemViewModelBase>();
            CardClickedCommand = new GalaSoft.MvvmLight.Command.RelayCommand<CardItemViewModelBase>(OnCardClicked);
            ListRight = new RelayCommand(OnCardListRightCallback);
            ListLeft = new RelayCommand(OnCardListLeftCallback);
            InitData();
        }

        private ICommand _cardClickedCommand;

        /// <summary>
        /// Каталог с полным набором карт
        /// </summary>
        public Catalog Catalog;

        /// <summary>
        /// Максимальное количество карт отображаемое на странице
        /// </summary>
        //TODO: DEFINE GLOBAL CONFIG
        private const int MaxCardDiplayCount = 8;

        /// <summary>
        /// Количество страниц с картами для генерала
        /// </summary>
        private int _pagesForGeneral;

        private int _currentPage;
        private List<CardItemViewModelBase> _displayedCardViewModels;
        private CardGeneral _selectedGeneral;

        /// <summary>
        /// Индекс текущей страницы
        /// </summary>
        private int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                ChangeDisplayCards(value, MaxCardDiplayCount, SelectedGeneral);
            }
        }

        /// <summary>
        /// Текущий выбранный генерал
        /// </summary>
        public CardGeneral SelectedGeneral
        {
            get { return _selectedGeneral; }
            private set
            {
                _selectedGeneral = value;
                value.IsSelected = true;
            }
        }

        /// <summary>
        /// Пролистнуть список карт в право
        /// </summary>
        public ICommand ListRight { get; set; }

        /// <summary>
        /// Пролистнуть список карт в лево
        /// </summary>
        public ICommand ListLeft { get; set; }

        public override void Cleanup()
        {
            base.Cleanup();
            foreach (var cardGeneral in Generals)
            {
                cardGeneral.IAmSelected -= OnGeneralSelectionChanged;
            }
        }

        private void InitData()
        {
            if (Generals.Count > 0)
            {
                foreach (var cardGeneral in Generals)
                {
                    cardGeneral.IAmSelected += OnGeneralSelectionChanged;
                }
                SelectedGeneral = Generals.First();
            }
        }

        private void OnGeneralSelectionChanged(CardGeneral general)
        {
            //Отключить выделение у всех остальных генералов
            Generals.Where(g => !g.Equals(SelectedGeneral))
                .ToList().ForEach(g=> g.IsSelected = false);
            _pagesForGeneral =
                Convert.ToInt32(Math.Ceiling((double) (SelectedGeneral.CardViewModels.Length/MaxCardDiplayCount)));
            CurrentPage = 0;
        }

        /// <summary>
        /// Изменить отображаемые карты
        /// </summary>
        private void ChangeDisplayCards(int currentPage, int cardOnPage, CardGeneral selecCardGeneral)
        {
            if (selecCardGeneral == null)
            {
                return;
            }

            var lastIdx = (currentPage + 1) * cardOnPage;
            var firstIdx = lastIdx - cardOnPage;

            DisplayedCardViewModels = selecCardGeneral.CardViewModels.Skip(firstIdx).Take(cardOnPage).ToList();
        }

        private void OnCardClicked(CardItemViewModelBase item)
        {
            //Произвести добавление карты в колоду            
        }

        public ICommand CardClickedCommand
        {
            get { return _cardClickedCommand; }
            set { _cardClickedCommand = value; }
        }

        /// <summary>
        /// Карты добавленные в колоду
        /// </summary>
        public ObservableCollection<CardItemViewModelBase> DeckCardItems { get; set; } 

        /// <summary>
        /// Доступные генералы
        /// </summary>
        public ObservableCollection<CardGeneral> Generals { get; set; }

        /// <summary>
        /// Карты, отображаемые на доске для выбора
        /// </summary>
        public List<CardItemViewModelBase> DisplayedCardViewModels
        {
            get { return _displayedCardViewModels; }
            set
            {
                _displayedCardViewModels = value;
                RaisePropertyChanged(() => DisplayedCardViewModels);
            }
        }

        /// <summary>
        /// Пролистнуть карты в лево
        /// </summary>
        private void OnCardListLeftCallback()
        {
            if (CurrentPage == 0)
            {
                //Если достигнута первая для генерала страница
                var generalIndex = TakeSelectedGeneralIndex();
                if (generalIndex != 0)
                {
                    //Если выбран не первый из генералов
                    SelectedGeneral = Generals[generalIndex - 1];
                    CurrentPage = _pagesForGeneral;
                }
            }
            else
            {
                //Если страница не первая
                CurrentPage--;
            }
        }

        /// <summary>
        /// Пролистнуть карты в право
        /// </summary>
        private void OnCardListRightCallback()
        {
            if (_pagesForGeneral == CurrentPage)
            {
                //Если достигнута последняя доступная для генерала страница
                var generalIndex = TakeSelectedGeneralIndex();
                if (generalIndex < Generals.Count - 1)
                {
                    //Если выбран не последний генерал, то перелистнуть на следующего по счету генерала
                    SelectedGeneral = Generals[generalIndex + 1];
                }
            }
            else
            {
                //Если страница не последняя
                CurrentPage++;
            }
        }

        private int TakeSelectedGeneralIndex()
        {
            if (SelectedGeneral != null)
            {
                var idx =  Generals.IndexOf(SelectedGeneral);
                if (idx == -1)
                {
                    throw new Exception("General not found in collection");
                }
                return idx;
            }
            throw new Exception("General does not selected");
        }

        private void InitCardChartInfoCollection()
        {
            ChartCardInfoCollection = new ObservableCollection<ChartBarCardInfo>();
            ChartCardInfoCollection.Add(new ChartBarCardInfo() { Count = 0, ManaCost = 0});
            ChartCardInfoCollection.Add(new ChartBarCardInfo() { Count = 1, ManaCost = 1 });
            ChartCardInfoCollection.Add(new ChartBarCardInfo() { Count = 2, ManaCost = 2 });
            ChartCardInfoCollection.Add(new ChartBarCardInfo() { Count = 3, ManaCost = 3 });
            ChartCardInfoCollection.Add(new ChartBarCardInfo() { Count = 4, ManaCost = 4 });
            ChartCardInfoCollection.Add(new ChartBarCardInfo() { Count = 5, ManaCost = 5 });
            ChartCardInfoCollection.Add(new ChartBarCardInfo() { Count = 6, ManaCost = 6 });
            ChartCardInfoCollection.Add(new ChartBarCardInfo() { Count = 7, ManaCost = 7 });
            ChartCardInfoCollection.Add(new ChartBarCardInfo() { Count = 8, ManaCost = 8 });
            ChartCardInfoCollection.Add(new ChartBarCardInfo() { Count = 9, ManaCost = 9 });
            ChartCardInfoCollection.Add(new ChartBarCardInfo() { Count = 10, ManaCost = 10 });
        }

        public ObservableCollection<ChartBarCardInfo> ChartCardInfoCollection { get; set; } 
    }
}
