using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;
using Duelyst.DeckConstructor.CardCatalog;
using Duelyst.DeckConstructor.ViewModel.DeckCardItem;
using GalaSoft.MvvmLight.Command;

namespace Duelyst.DeckConstructor.ViewModel
{
    public class DeckConstructorViewModel : ResizableViewModelBase
    {
        private ICommand _cardClickedCommand;

        /// <summary>
        /// Каталог с полным набором карт
        /// </summary>
        public CardsCatalog Catalog;

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
        public CardGeneral SelectedGeneral { get; private set; }


        public DeckConstructorViewModel()
        {
            Catalog = CardsCatalog.Instance;
            InitCardChartInfoCollection();
            DeckCardItems = new ObservableCollection<CardItemViewModelBase>();
            Generals = new ObservableCollection<CardGeneral>(Catalog.ViewModelGenerals);
            DisplayedCardViewModels = new ObservableCollection<CardItemViewModelBase>();
            CardClickedCommand = new RelayCommand<CardItemViewModelBase>(OnCardClicked);
            InitData();
        }

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
                Generals.First().IsSelected = true;
            }
        }

        private void OnGeneralSelectionChanged(CardGeneral general)
        {
            SelectedGeneral = general;
            //Отключить выделение у всех остальных генералов
            Generals.Where(g => !g.Equals(SelectedGeneral))
                .ToList().ForEach(g=> g.IsSelected = false);
            _pagesForGeneral =
                Convert.ToInt32(Math.Ceiling((double) (SelectedGeneral.CardViewModels.Count/MaxCardDiplayCount)));
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
            if (lastIdx > selecCardGeneral.CardViewModels.Count)
            {
                lastIdx = selecCardGeneral.CardViewModels.Count;
            }

            DisplayedCardViewModels.Clear();
            for (var idx = firstIdx; idx < lastIdx; idx++)
            {
                DisplayedCardViewModels.Add(selecCardGeneral.CardViewModels[idx]);
            }
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
        public ObservableCollection<CardItemViewModelBase> DisplayedCardViewModels { get; set; }


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
