using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Duelyst.DeckConstructor.CardCatalog;
using GalaSoft.MvvmLight;

namespace Duelyst.DeckConstructor.ViewModel.DeckCardItem
{
    public class CardItemViewModelBase : ListItemViewModelBase
    {
        public CardItemViewModelBase(string name)
        {
            Name = name;
            Image = new BitmapImage(new Uri("pack://application:,,,/Duelyst.DeckConstructor;component/Pages/UI/NO_CARD_DAT.png"));
            MaxInDeck = 3;
            AlreadyAdded = 0;
        }

        public void SetImage(BitmapImage source)
        {
            if (source != null)
            {
                Image = source;
            }   
        }

        public string CardId { get; set; }

        public int MaxInDeck { get; set; }

        public ECardType CardType { get; set; }


    }
}
