using System;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;

namespace Duelyst.DeckConstructor.ViewModel.DeckCardItem
{
    public class DeckCardItemViewModel : ViewModelBase
    {
        public DeckCardItemViewModel(int manaCost, string name)
        {
            ManaCost = manaCost;
            Name = name;
            Image = new BitmapImage(new Uri("pack://application:,,,/Duelyst.DeckConstructor;component/Pages/UI/NO_CARD_DAT.png"));
            MaxInDeck = 3;
            AlreadyAdded = 0;
        }

        public int ManaCost { get; set; }

        public string Name { get; set; }

        public BitmapImage Image { get; set; }

        public int MaxInDeck { get; set; }

        public int AlreadyAdded { get; set; }


    }
}
