using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Duelyst.DeckConstructor.CardCatalog;
using GalaSoft.MvvmLight;

namespace Duelyst.DeckConstructor.ViewModel.DeckCardItem
{
    public class CardItemViewModelBase : ViewModelBase
    {
        public CardItemViewModelBase(string name)
        {
            Name = name;
            Image = new BitmapImage(new Uri("pack://application:,,,/Duelyst.DeckConstructor;component/Pages/UI/NO_CARD_DAT.png"));
            MaxInDeck = 3;
            AlreadyAdded = 0;
        }

        /// <summary>
        /// Стоимость 
        /// </summary>
        public int ManaCost { get; set; }

        public string Name { get; set; }

        public void SetImage(BitmapImage source)
        {
            if (source != null)
            {
                Image = source;
            }   
        }

        public BitmapImage Image { get; private set; }

        public int MaxInDeck { get; set; }

        public int AlreadyAdded { get; set; }

        public ECardType CardType { get; set; }


    }
}
