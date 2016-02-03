using System;
using System.Windows.Media.Imaging;
using Duelyst.DeckConstructor.CardCatalog;

namespace Duelyst.DeckConstructor.ViewModel.DeckCardItem
{
    public class CardItemViewModelBase : ListItemViewModelBase, IEquatable<CardItemViewModelBase>
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

        public CardItemViewModelBase Owner { get; set; }

        public string CardId { get; set; }

        public int MaxInDeck { get; set; }

        public ECardType CardType { get; set; }


        public bool Equals(CardItemViewModelBase other)
        {
            return other != null && other.CardId == CardId;
        }
    }
}
