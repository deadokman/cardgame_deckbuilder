using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
                "CardImageSource",
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

        public BitmapImage CardImageSource
        {
            get { return (BitmapImage)GetValue(imageSourceProperty); }
            set
            {
                SetValue(imageSourceProperty, value);
            }
        }

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

        private static DoubleAnimation GetAnimation(int msec, double from, double to)
        {
            var growAnimation = new DoubleAnimation();
            growAnimation.Duration = TimeSpan.FromMilliseconds(msec);
            growAnimation.From = from;
            growAnimation.To = to;
            return growAnimation;
        }

    }
}
