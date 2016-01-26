using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;

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
                "CardSource",
                typeof(BitmapImage), typeof(SinglecardView));

        private static DependencyProperty cardDeckcountProperty = DependencyProperty.Register(
                "IndeckCount",
                typeof(string), typeof(SinglecardView));

        public SinglecardView()
        {
            InitializeComponent();
        }

        public string IndeckCount
        {
            get { return (string)GetValue(cardDeckcountProperty); }
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

        private static ICommand _clickCommand;

        public event EventHandler CardClickedHandler;

        private void RaiseClicked()
        {
            if (CardClickedHandler != null)
            {
                CardClickedHandler(this, new AddingNewItemEventArgs());
            }
        }

        private void CardImage_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            RaiseClicked();
        }
    }
}
