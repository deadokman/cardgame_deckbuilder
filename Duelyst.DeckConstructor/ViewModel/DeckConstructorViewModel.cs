using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duelyst.DeckConstructor.ViewModel.DeckCardItem;

namespace Duelyst.DeckConstructor.ViewModel
{
    public class DeckConstructorViewModel : ResizableViewModelBase
    {
        public DeckConstructorViewModel()
        {
            InitCardChartInfoCollection();
            DeckCardItems = new ObservableCollection<DeckCardItemViewModel>();
            Generals = new ObservableCollection<CardGeneral>();
            InitData();
        }

        private void InitData()
        {
            //TestData
            DeckCardItems.Add(new DeckCardItemViewModel(0, "TESTC1"));
            DeckCardItems.Add(new DeckCardItemViewModel(2, "TESTC3"));
            //
            Generals.Add(new CardGeneral("GENERAL1"));
        }

        public ObservableCollection<DeckCardItemViewModel> DeckCardItems { get; set; } 

        public ObservableCollection<CardGeneral> Generals { get; set; } 

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
