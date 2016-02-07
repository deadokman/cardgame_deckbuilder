using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Duelyst.DeckConstructor.CardCatalog;
using Duelyst.DeckConstructor.ViewModel.Communication;
using Duelyst.DeckConstructor.ViewModel.DeckCardItem;
using Duelyst.DeckConstructor.ViewModel.Ifaces.CardDisplayObjects;
using GalaSoft.MvvmLight.CommandWpf;

namespace Duelyst.DeckConstructor.ViewModel
{
    public class DeckConstructorViewModel : ResizableViewModelBase
    {
        /// <summary>
        /// Доступные генералы
        /// </summary>
        public ObservableCollection<IDisplayableFilter> CardFilters
        {
            get { return _cardFilters; }
            set
            {
                _cardFilters = value; 
                RaisePropertyChanged(() => CardFilters);
            }
        }

        /// <summary>
        /// Максимальное количество карт отображаемое на странице
        /// </summary>
        //TODO: DEFINE GLOBAL CONFIG
        private const int MaxCardDiplayCount = 8;

        /// <summary>
        /// Каталог с полным набором карт
        /// </summary>
        public Catalog Catalog;

        /// <summary>
        /// Активны кнопки навигирования
        /// </summary>
        public bool NavigationButtonsEnabled
        {
            get { return _navigationButtonsEnabled; }
            set
            {
                _navigationButtonsEnabled = value;
                RaisePropertyChanged(() => NavigationButtonsEnabled);
            }
        }

        private bool _navigationButtonsEnabled;

        /// <summary>
        /// Пролистнуть список карт в право
        /// </summary>
        public ICommand ListRight { get; set; }

        /// <summary>
        /// Пролистнуть список карт в лево
        /// </summary>
        public ICommand ListLeft { get; set; }


        /// <summary>
        /// Индекс текущей страницы
        /// </summary>
        private int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                var preview = PreviewCurrentPageIndexChanged(value);
                _currentPage = preview;
                DisplayFromFilter(_selectedFilter);
                RaisePropertyChanged(() => CurrentPage);
            }
        }

        private int _currentPage;

        /// <summary>
        /// Объекты к отображению
        /// </summary>
        public List<IDisplayadble> ItemsToDisplay
        {
            get { return _itemsToDisplay; }
            set
            {
                _itemsToDisplay = value;
                RaisePropertyChanged(() => ItemsToDisplay);
            }
        }

        /// <summary>
        /// Максимальное количество страниц для выбранного фильтра
        /// </summary>
        public int PagesTotalForSelected
        {
            get
            {
                return _selectedFilter == null ? 0 : 
                    Convert.ToInt32(Math.Ceiling((decimal)_selectedFilter.ChildData.Count / MaxCardDiplayCount));
            }
        }

        private List<IDisplayadble> _itemsToDisplay;

        private IDisplayableFilter _selectedFilter;
        private ObservableCollection<IDisplayableFilter> _cardFilters;


        public DeckConstructorViewModel()
        {
            Catalog = Catalog.Instance;
            //Фильтрами служат генералы
            ItemsToDisplay = new List<IDisplayadble>();
            CardClickedCommand = new GalaSoft.MvvmLight.Command.RelayCommand<CardItemViewModelBase>(OnCardClicked);
            ListRight = new RelayCommand(OnCardListRightCallback);
            ListLeft = new RelayCommand(OnCardListLeftCallback);
            MessengerInstance.Register<IDisplayStrategy>(this, OnNavMessageRecive);
            NavigationButtonsEnabled = true;
        }

        public void SetDisplayFilters(IEnumerable<IDisplayableFilter> filters)
        {
            UnbindFilters();
            CardFilters = new ObservableCollection<IDisplayableFilter>(filters);
            BindFilters();
            if (CardFilters.Any())
            {
                var first = CardFilters.First();
                first.IsSelected = true;
                _selectedFilter = first;
            }
            SetListToDisplay();
        }


        /// <summary>
        /// Перестроить список объектов к отображению
        /// Для отображения доступны только модели из активного фильтра
        /// </summary>
        public void SetListToDisplay()
        {
            var availebleFilters = CardFilters.Where(f => f.IsAvailebleToSelect).ToArray();
            if (availebleFilters.Any())
            {
                availebleFilters[0].IsSelected = true;
            }
            CurrentPage = 0;
        }

        /// <summary>
        /// Вызывается, когда выбранный фильтр изменен
        /// </summary>
        private void DisplayFromFilter(IDisplayableFilter selected)
        {
            if (selected == null)
            {
                return;
            }

            var itemFirstToSelect = CurrentPage * MaxCardDiplayCount;
            ItemsToDisplay = selected.ChildData.Skip(itemFirstToSelect).Take(MaxCardDiplayCount).ToList();
        }

        /// <summary>
        /// Реакция на выбранный фильтр
        /// </summary>
        /// <param name="filter"></param>
        private void FilterOnSelectedChanged(IDisplayableFilter filter)
        {
            //if (filter.IsSelected)
            //{
            //    _selectedFilter = filter;
            //    DisplayFromFilter(filter);
            //}
        }

        private int PreviewCurrentPageIndexChanged(int page)
        {
            //Если выбрана страница больше чем доступна для текущего фильтра
            int pageSelected;
            if (page == PagesTotalForSelected)
            {
                var idx = CardFilters.IndexOf(_selectedFilter);
                if (idx < CardFilters.Count -1)
                {
                    _selectedFilter = CardFilters[idx + 1];
                    pageSelected = 0;
                    CardFilters[idx + 1].IsSelected = true;
                    return pageSelected;
                }
                pageSelected = PagesTotalForSelected - 1;
                return pageSelected;
            }
            if(page < 0)
            {
                var idx = CardFilters.IndexOf(_selectedFilter);
                if (idx != 0)
                {
                    _selectedFilter = CardFilters[idx - 1];
                    pageSelected = Convert.ToInt32(Math.Ceiling((decimal)_selectedFilter.ChildData.Count / MaxCardDiplayCount)) - 1;
                    _selectedFilter.IsSelected = true;
                    return pageSelected;
                }
                pageSelected = 0;
                return pageSelected;
            }
            pageSelected = page;
            return pageSelected;
        }

        /// <summary>
        /// Пролистнуть карты в лево
        /// </summary>
        private void OnCardListLeftCallback()
        {
            CurrentPage = CurrentPage - 1;
        }

        /// <summary>
        /// Пролистнуть карты в право
        /// </summary>
        private void OnCardListRightCallback()
        {
            CurrentPage = CurrentPage + 1;
        }

        private void OnNavMessageRecive(IDisplayStrategy message)
        {
            if (message != null)
            {
                SetDisplayFilters(message.GetStrategyFilters());
            }
        }

        /// <summary>
        /// Биндинг на команду клика по карте
        /// </summary>
        public ICommand CardClickedCommand { get; set; }

        /// <summary>
        /// Реакция на клик по карте
        /// </summary>
        /// <param name="item"></param>
        private void OnCardClicked(CardItemViewModelBase item)
        {
            MessengerInstance.Send<CardClickMessage>(new CardClickMessage(item));
        }

        private void BindFilters()
        {
            foreach (var filter in CardFilters)
            {
                filter.Selected += FilterOnSelectedChanged;
            }
        }

        private void UnbindFilters()
        {
            if (CardFilters != null)
            {
                foreach (var filter in CardFilters)
                {
                    filter.Selected -= FilterOnSelectedChanged;
                }
            }
        }

        public override void Cleanup()
        {
            UnbindFilters();
        }
    }

}
