using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Duelyst.DeckConstructor.Pages
{
    /// <summary>
    /// Interaction logic for SinglecardView.xaml
    /// </summary>
    public partial class SinglecardView : UserControl
    {
        // Статическое свойство только для чтения DependencyProperty.
        private static DependencyProperty imageSourceProperty = 
                DependencyProperty.Register(
                "CardImageSource",
                typeof(Uri), typeof(SinglecardView));

        private static DependencyProperty cardDeckcountProperty = DependencyProperty.Register(
                "IndeckCount",
                typeof(string), typeof(SinglecardView));

        public SinglecardView()
        {
            InitializeComponent();
        }

        public string IndeckCount
        {
            get { return (string)GetValue(cardDeckcountProperty);}
            set
            {
                SetValue(cardDeckcountProperty, value);
            }
        }

        public BitmapImage CardSource
        {
            get { return (BitmapImage)GetValue(imageSourceProperty); }
            set
            {
                SetValue(imageSourceProperty, value);
            }
        }

    }
}
