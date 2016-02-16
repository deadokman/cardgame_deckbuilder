using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;

namespace Duelyst.DeckConstructor.ViewModel.DeckCardItem
{

    public class ListItemViewModelBase : ViewModelBase
    {
        [XmlIgnore]
        public BitmapImage CuttedImage { get; protected set; }

        [XmlIgnore]
        public string Name { get; set; }

        [XmlIgnore]
        public bool IsShowManaCost { get; set; }

        [XmlIgnore]
        public bool IsShowInDeckCount{ get; set; }

        /// <summary>
        /// Стоимость 
        /// </summary>
        [XmlIgnore]
        public int ManaCost { get; set; }

        [XmlIgnore]
        public int AlreadyAdded { get; set; }

    }
}
