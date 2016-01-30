using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;

namespace Duelyst.DeckConstructor.ViewModel.DeckCardItem
{
    public class CardListItemViewModelBase : ViewModelBase
    {
        public BitmapImage Image { get; protected set; }

        public BitmapImage CuttedImage { get; protected set; }

        public string Name { get; set; }

        public bool IsShowManaCost { get; set; }

        public bool IsShowInDeckCount{ get; set; }

        /// <summary>
        /// Стоимость 
        /// </summary>
        public int ManaCost { get; set; }


        public int AlreadyAdded { get; set; }

    }
}
