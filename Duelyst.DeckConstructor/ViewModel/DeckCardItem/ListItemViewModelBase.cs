using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;

namespace Duelyst.DeckConstructor.ViewModel.DeckCardItem
{

    public class ListItemViewModelBase : ViewModelBase
    {
        /// <summary>
        /// Обрезанная картинка для отображения в списке
        /// </summary>
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
