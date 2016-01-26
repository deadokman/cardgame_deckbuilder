using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Input;
using Duelyst.DeckConstructor.ViewModel.DeckCardItem;
using GalaSoft.MvvmLight.Command;

namespace Duelyst.DeckConstructor.ViewModel
{
    public class DeckConstructorViewModel : ResizableViewModelBase
    {
        private ICommand _cardClickedCommand;

        public DeckConstructorViewModel()
        {
            InitCardChartInfoCollection();
            DeckCardItems = new ObservableCollection<DeckCardItemViewModel>();
            Generals = new ObservableCollection<CardGeneral>();
            DisplayedCardViewModels = new ObservableCollection<DeckCardItemViewModel>();
            CardClickedCommand = new RelayCommand<DeckCardItemViewModel>(OnCardClicked);
            InitData();
        }

        private void InitData()
        {
            //
            DisplayedCardViewModels.Add(new DeckCardItemViewModel(0, "TEST_CARD123"));
            DisplayedCardViewModels.Add(new DeckCardItemViewModel(0, "TEST_CARD12"));
            DisplayedCardViewModels.Add(new DeckCardItemViewModel(0, "TEST_CARD156"));
            Generals.Add(new CardGeneral("GENERAL1"));
        }


        private void OnCardClicked(DeckCardItemViewModel item)
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
        public ObservableCollection<DeckCardItemViewModel> DeckCardItems { get; set; } 

        /// <summary>
        /// Доступные генералы
        /// </summary>
        public ObservableCollection<CardGeneral> Generals { get; set; } 

        /// <summary>
        /// Карты, отображаемые на доске для выбора
        /// </summary>
        public ObservableCollection<DeckCardItemViewModel> DisplayedCardViewModels { get; set; }


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
